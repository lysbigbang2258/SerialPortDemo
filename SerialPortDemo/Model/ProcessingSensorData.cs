// 2019061415:27

namespace SerialPortDemo.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight;

    public sealed class ProcessingSensorData : ObservableObject
    {
        #region Filed

        private const int THRESHVALUE = 14000;
        private static readonly object Rcvlock = new object();
        private static readonly object SendLock = new object();

        private string[] portsNames;
        private SerialPort mySerialPort;
        private List<byte> receivesBuffer;

        private bool isCollected;
        private bool isPortOpen;
        private int rcvPackets;

        private int rcvRate;

        private int samplingFreq;

        private int saveFreq;

        private int sendPackets;

        private bool isAutoSave;

        private int addressNum;

        /// <summary>
        /// The send command timer.
        /// </summary>
        private DispatcherTimer autoSendDataTimer;

        /// <summary>
        /// The check rcv data timer.
        /// </summary>
        private DispatcherTimer checkTimer;

        /// <summary>
        /// The address family.
        /// </summary>
        private List<int> addressFamily;

        /// <summary>
        /// The received buffer.
        /// </summary>
        private List<byte> receivedList;

        /// <summary>
        /// The should clear buffer.
        /// </summary>
        private bool shouldClearBuffer;

        /// <summary>
        ///     The rcv event handler.
        /// </summary>
        public event EventHandler<SensorEventArgs> SendSensorEventHandler;

        /// <summary>
        /// The save event handler.
        /// </summary>
        public event EventHandler<SensorEventArgs> SaveSensorEventHandler;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingSensorData" /> class.
        /// </summary>
        public ProcessingSensorData()
        {
            Init();
        }

        #region Property

        /// <summary>
        /// Gets or sets a value indicating whether is open.
        /// </summary>
        public bool IsOpenPort {
            get => isPortOpen;

            set {
                isPortOpen = value;
                RaisePropertyChanged(() => IsOpenPort);
            }
        }

        /// <summary>
        /// Gets and Sets a value indicating whether is Collect.
        /// </summary>
        public bool IsCollected {
            get => isCollected;
            set {
                isCollected = value;
                RaisePropertyChanged(() => IsCollected);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is auto save.
        /// </summary>
        public bool IsAutoSave {
            get => isAutoSave;

            set {
                isAutoSave = value;
                RaisePropertyChanged(() => IsAutoSave);
            }
        }

        /// <summary>
        /// Gets or sets the address num.
        /// </summary>
        public int AddressNum {
            get => addressNum;
            set {
                addressNum = value;
                RaisePropertyChanged(AddressNum.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the rcv packets.
        /// </summary>
        public int RcvPackets {
            get => rcvPackets;

            set {
                rcvPackets = value;
                RaisePropertyChanged(() => RcvPackets);
            }
        }

        /// <summary>
        /// Gets or sets the rcv rate.
        /// </summary>
        public int RcvRate {
            get => rcvRate;

            set {
                rcvRate = value;
                RaisePropertyChanged(() => RcvRate);
            }
        }

        /// <summary>
        /// Gets or sets the sampling freq.
        /// </summary>
        public int SamplingFreq {
            get => samplingFreq;

            set {
                samplingFreq = value;
                RaisePropertyChanged(() => SamplingFreq);
            }
        }

        /// <summary>
        /// Gets or sets the save freq.
        /// </summary>
        public int SaveFreq {
            get => saveFreq;
            set {
                saveFreq = value;
                RaisePropertyChanged(() => SaveFreq);
            }
        }

        /// <summary>
        /// Gets or sets the send packets.
        /// </summary>
        public int SendPackets {
            get => sendPackets;

            set {
                sendPackets = value;
                RaisePropertyChanged(() => SendPackets);
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// The init.
        /// </summary>
        private void Init()
        {
            portsNames = null;
            addressFamily = new List<int>();
            receivedList = new List<byte>();
            receivesBuffer = new List<byte>();

            InitCheckTimer();

            InitAutoSendTimer();
        }

        /// <summary>
        /// The init auto send timer.
        /// </summary>
        private void InitAutoSendTimer()
        {
            autoSendDataTimer = new DispatcherTimer { IsEnabled = false };
            autoSendDataTimer.Tick += AutoSendDataTimerTick;
        }

        /// <summary>
        /// The auto send data timer tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AutoSendDataTimerTick(object sender, EventArgs e)
        {
            if (addressFamily.Count == 0)
            {
                return;
            }

            foreach (int i in addressFamily)
            {
                lock (SendLock)
                {
                    bool ret = SendCommand(i); // ToDo
                    if (ret == false)
                    {
                        StopAutoSendDataTimer();
                    }
                }

                SendPackets++;
                Thread.Sleep(SamplingFreq);

                Console.WriteLine("发送数据");
            }
        }

        /// <summary>
        /// The stop auto send data timer.
        /// </summary>
        private void StopAutoSendDataTimer()
        {
            autoSendDataTimer.IsEnabled = false;
            autoSendDataTimer.Stop();
        }

        /// <summary>
        /// The init check timer.
        /// </summary>
        private void InitCheckTimer()
        {
            // 如果缓冲区中有数据，并且定时时间达到前依然没有得到处理，将会自动触发处理函数。
            checkTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 50), IsEnabled = false };
            checkTimer.Tick += CheckTimerTick;
        }

        /// <summary>
        /// The check timer tick.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void CheckTimerTick(object sender, EventArgs e)
        {
            // 触发了就把定时器关掉，防止重复触发。
            StopCheckTimer();

            if (receivesBuffer.Count < 14)
            {
                return;
            }

            // 只有没有到达阈值的情况下才会强制其启动新的线程处理缓冲区数据。
            if (receivesBuffer.Count < THRESHVALUE)
            {
                // 进行数据处理，采用新的线程进行处理。
                Thread dataHandler = new Thread(BufferReceived);
                dataHandler.Start();
            }
        }

        /// <summary>
        /// The stop check timer.
        /// </summary>
        private void StopCheckTimer()
        {
            checkTimer.IsEnabled = false;
            checkTimer.Stop();
        }

        /// <summary>
        /// The start Rcv Data.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool StartRcvData()
        {
            if (IsOpenPort)
            {
                mySerialPort.DataReceived += PortRcvByteReached;
                return true;
            }

            return false;
        }

        /// <summary>
        /// The stop Rcv Data.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool StopRcvData()
        {
            if (IsCollected)
            {
                IsCollected = false;
                mySerialPort.DataReceived -= PortRcvByteReached;
                return true;
            }

            return false;
        }

        /// <summary>
        /// The send data.
        /// </summary>
        /// <param name="dataBytes">
        ///     command bytes
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SendBytes(byte[] dataBytes)
        {
            try
            {
                if (!IsOpenPort || !IsCollected)
                {
                    return false;
                }

                mySerialPort.Write(buffer: dataBytes, 0, count: dataBytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(value: e);
            }

            return true; // Todo
        }

        /// <summary>
        ///     The send angle command.
        /// </summary>
        /// <param name="index">
        ///     The index.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SendCommand(int index)
        {
            byte[] angle = { 0x77, 0x04, 0x00, 0x04, 0x08 };
            angle[2] += (byte)index;
            angle[4] += (byte)index;

            return SendBytes(dataBytes: angle);
        }

        /// <summary>
        /// The start send timer.
        /// </summary>
        /// <param name="interval">
        /// The interval.
        /// </param>
        private void StartSendTimer(int interval)
        {
            autoSendDataTimer.IsEnabled = true;
            autoSendDataTimer.Interval = TimeSpan.FromMilliseconds(interval);
            autoSendDataTimer.Start();
        }

        /// <summary>
        /// The collected data.
        /// </summary>
        /// <param name="addresses">
        /// The addresses.
        /// </param>
        public void CollectedData(Dictionary<int, bool> addresses)
        {
            if (IsCollected)
            {
                return;
            }

            IsCollected = true;

            List<int> list = new List<int>();

            foreach (KeyValuePair<int, bool> key_value_pair in addresses)
            {
                if (key_value_pair.Value)
                {
                    list.Add(key_value_pair.Key + 1);
                }
            }

            Task.Run(
                () =>
                    {
                        foreach (int i in list)
                        {
                            SendCommand(i);
                            SendPackets++;
                            Thread.Sleep(100);
                        }

                        list.Clear();
                        IsCollected = false;
                    });
        }

        /// <summary>
        /// The collect data.
        /// </summary>
        /// <param name="addresses">
        /// The addresses.
        /// </param>
        /// <param name="period">发送定期器周期</param>
        public void StartAutoCollectData(Dictionary<int, bool> addresses, int period = 1000)
        {
            if (IsCollected)
            {
                return;
            }

            IsCollected = true;

            foreach (KeyValuePair<int, bool> key_value_pair in addresses)
            {
                if (key_value_pair.Value)
                {
                    addressFamily.Add(key_value_pair.Key + 1);
                }
            }

            if (addressFamily.Count == 0)
            {
                return;
            }

            // 启动自动发送定时器
            StartSendTimer(SamplingFreq); // Todo 
        }

        /// <summary>
        /// The stop collect data.
        /// </summary>
        public void StopCollectData()
        {
            StopRcvData();
            StopAutoSendDataTimer();
            StopCheckTimer();
            addressFamily.Clear();
        }

        /// <summary>
        /// The set port param.
        /// </summary>
        /// <param name="portName">
        ///     The portName.
        /// </param>
        /// <param name="baudRate">
        ///     The baudRate.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SetPortParam(string portName = "COM3", int baudRate = 19200)
        {
            if (portName == null || portName.Length <= 0 || baudRate <= 0)
            {
                return false;
            }

            portsNames = SerialPort.GetPortNames();
            if (!TestPortName(name: portName))
            {
                return false;
            }

            mySerialPort = SetPortProperty(portName: portName, baudRate: baudRate, parity: Parity.None, stopBits: StopBits.One, 14);

            return true;
        }

        /// <summary>
        /// The set port property.
        /// </summary>
        /// <param name="portName">
        /// The port name.
        /// </param>
        /// <param name="baudRate">
        /// The baud rate.
        /// </param>
        /// <param name="parity">
        /// The parity.
        /// </param>
        /// <param name="stopBits">
        /// The stop bits.
        /// </param>
        /// <param name="threshold">
        /// The threshold.
        /// </param>
        /// <returns>
        /// The <see cref="SerialPort"/>.
        /// </returns>
        private SerialPort SetPortProperty(string portName, int baudRate, Parity parity, StopBits stopBits, int threshold)
        {
            if (baudRate <= 0)
            {
                return null;
            }

            return new SerialPort
                       {
                           PortName = portName,
                           BaudRate = baudRate,
                           Parity = parity,
                           StopBits = stopBits,
                           ReceivedBytesThreshold = threshold,
                           RtsEnable = true
                       };
        }

        /// <summary>
        /// The test port name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool TestPortName(string name)
        {
            if (name == null || name.Length <= 0)
            {
                return false;
            }

            foreach (string s in portsNames)
            {
                if (s.Equals(value: name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The close port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool ClosePort()
        {
            // 等待个20毫秒，以确保不再接收，在关闭串口
            // 否则，如果频繁点击打开/关闭 串口还在接收数据就关闭串口会出现界面卡死
            Thread.Sleep(10);
            mySerialPort.Close();
            IsOpenPort = mySerialPort.IsOpen;
            return !mySerialPort.IsOpen;
        }

        /// <summary>
        ///     The open port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool OpenPort()
        {
            try
            {
                mySerialPort.Open();
                IsOpenPort = mySerialPort.IsOpen;
                return IsOpenPort;
            }
            catch (Exception e)
            {
                Console.WriteLine(value: e);
                return false;
            }
        }

        /// <summary>
        /// The get rcv data.
        /// </summary>
        /// <param name="srcBytes">
        /// The src bytes.
        /// </param>
        /// <param name="angles">
        /// The angles.
        /// </param>
        private void GetRcvData(byte[] srcBytes, out Angles angles)
        {
            if (srcBytes == null)
            {
                angles = new Angles(0, 0, 0);
                return;
            }

            if (srcBytes.Length != 14)
            {
                angles = new Angles(0, 0, 0);
                return;
            }

            if (srcBytes[0] != 0x77 || srcBytes[1] != 0x0d || srcBytes[3] != 0x84)
            {
                angles = new Angles(0, 0, 0);
                return;
            }

            var h_bytes = new byte[3];
            var p_bytes = new byte[3];
            var r_bytes = new byte[3];

            Array.Copy(sourceArray: srcBytes, 4, destinationArray: h_bytes, 0, 3);
            Array.Copy(sourceArray: srcBytes, 7, destinationArray: p_bytes, 0, 3);
            Array.Copy(sourceArray: srcBytes, 10, destinationArray: r_bytes, 0, 3);

            double heading = GetDoubleAngle(srcBytes: h_bytes);
            double pitch = GetDoubleAngle(srcBytes: p_bytes);
            double roll = GetDoubleAngle(srcBytes: r_bytes);

            angles = new Angles(head: heading, pitch: pitch, roll: roll);
        }

        /// <summary>
        /// The on save event handler.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnSaveEventHandler(SensorEventArgs e)
        {
            SaveSensorEventHandler?.Invoke(this, e);
        }

        /// <summary>
        ///  The get height 4.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        private int GetHeight4(byte data)
        {
            // 获取高四位
            int height = (data & 0xf0) >> 4;
            return height;
        }

        /// <summary>
        ///     The get low 4.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        private int GetLow4(byte data)
        {
            // 获取低四位
            int low = data & 0x0f;
            return low;
        }

        /// <summary>
        ///     The get double angle.
        /// </summary>
        /// <param name="srcBytes">
        ///     The src bytes.
        /// </param>
        /// <returns>
        ///     The <see cref="double" />.
        /// </returns>
        private double GetDoubleAngle(byte[] srcBytes)
        {
            double sign = GetHeight4(srcBytes[0]) == 0 ? 1 : -1;
            double high = (100 * GetLow4(srcBytes[0])) + (10 * GetHeight4(srcBytes[1]));
            double low = (1 * GetLow4(srcBytes[1])) + (0.1 * GetHeight4(srcBytes[2])) + (0.01 * GetLow4(srcBytes[2]));
            double result = sign * (high + low);
            return result;
        }

        /// <summary>
        /// The get port names.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public List<string> GetPortNames()
        {
            return new List<string>(SerialPort.GetPortNames());
        }

        /// <summary>
        /// The on rcv event handler.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void OnSendEventHandler(SensorEventArgs e)
        {
            SendSensorEventHandler?.Invoke(this, e: e);
        }

        /// <summary>
        ///     The port rcv byte reached.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void PortRcvByteReached(object sender, SerialDataReceivedEventArgs e)
        {
            if (!(sender is SerialPort sp))
            {
                return;
            }

            int bytesToRead = sp.BytesToRead;
            if (bytesToRead < 14)
            {
                Console.WriteLine("接收数据长度太短");
                return;
            }

            byte[] tempBuffer = new byte[bytesToRead];

            sp.Read(tempBuffer, 0, bytesToRead);

            if (tempBuffer[0] != 0x77)
            {
                Console.WriteLine("Read fram head wrong");
            }

            RcvPackets++;
            if (shouldClearBuffer)
            {
                lock (Rcvlock)
                {
                    receivesBuffer.Clear();
                }

                shouldClearBuffer = false;
            }

            // 暂存缓冲区字节到全局缓冲区中等待处理
            lock (Rcvlock)
            {
                receivesBuffer.AddRange(tempBuffer);
            }

            StartCheckTimer();
        }

        /// <summary>
        /// The start check timer.
        /// </summary>
        private void StartCheckTimer()
        {
            checkTimer.IsEnabled = true;
            checkTimer.Start();
        }

        /// <summary>
        /// The buffer received.
        /// </summary>
        private void BufferReceived()
        {
            checkTimer.Stop();
            var buffer = receivesBuffer;

            if (buffer[0] != 0x77 || buffer.Count < 14)
            {
                Console.WriteLine("RCV fram head wrong");
                receivesBuffer.Clear();
                return;
            }

            receivedList.AddRange(buffer);

            List<byte[]> send_list = new List<byte[]>();

            int count = receivedList.Count / 14;

            for (int i = 0; i < count; i++)
            {
                var tmp = receivedList.GetRange(14 * i, 14).ToArray();
                send_list.Add(tmp);
            }

            receivedList.RemoveRange(0, 14 * count);
            shouldClearBuffer = true;

            Console.WriteLine("Get send bytes");
            Angles angles = new Angles(0, 0, 0);

            foreach (byte[] t in send_list)
            {
                GetRcvData(t, out angles);
                int index = t[2];
                var arg = new SensorEventArgs(angles, index);
                OnSendEventHandler(arg);
                OnSaveEventHandler(arg);
            }

            send_list.Clear();
            Console.WriteLine("Send UI Data");
        }

        /// <summary>
        /// The init default property.
        /// </summary>
        public void InitDefaultProperty()
        {
            SendPackets = 0;
            RcvPackets = 0;
            addressFamily.Clear();
        }

        #endregion
    }
}