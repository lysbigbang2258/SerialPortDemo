// 2019061415:27

namespace SerialPortDemo.Model {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Threading;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight;

    /// <summary>
    ///     The rcv data proc.
    /// </summary>
    public class DataProcUnit : ObservableObject
    {
        #region Filed

        /// <summary>
        ///  The All ports name.
        /// </summary>
        string[] portsNames;

        /// <summary>
        ///   The my serial port resource.
        /// </summary>
        SerialPort mySerialPort;

        /// <summary>
        /// The rcv data lock.
        /// </summary>
        private object rcvLock;

        private bool isCollecting;

        private Angles anglesContent;

        private bool isOpenPortPort;

        private int rcvPackets;

        private int rcvRate;

        private int samplingFreq;

        private int saveFreq;

        private int sendPackets;

        private bool isAutoSave;

        /// <summary>
        ///     The rcv event handler.
        /// </summary>
        public event EventHandler<SensorEventArgs> SendEventHandler;
        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataProcUnit" /> class.
        /// </summary>
        public DataProcUnit() {
            RcvCQueue = new ConcurrentQueue<byte[]>();
            RcvList = new List<byte>();
            Init();
        }

        #region Property

        /// <summary>
        /// Gets or sets a value indicating whether is open.
        /// </summary>
        public bool IsOpenPort {
            get => isOpenPortPort;

            set {
                isOpenPortPort = value;
                RaisePropertyChanged(() => IsOpenPort);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Collect.
        /// </summary>
        public bool IsCollecting {
            get => isCollecting;
            set {
                isCollecting = value;
                RaisePropertyChanged(() => IsCollecting);
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
        /// Gets or sets the angles content.
        /// </summary>
        public Angles AnglesContent {
            get => anglesContent;
            set {
                anglesContent = value;
                RaisePropertyChanged(() => AnglesContent);
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

        /// <summary>
        ///     Gets the rcv c queue.
        /// </summary>
        ConcurrentQueue<byte[]> RcvCQueue {
            get;
        }

        /// <summary>
        ///     Gets the rcv list.
        /// </summary>
        List<byte> RcvList {
            get;
        }

        #endregion

        #region Method

        /// <summary>
        /// The init.
        /// </summary>
        void Init()
        {
            portsNames = null;
            mySerialPort = new SerialPort();
            rcvLock = new object();
        }

        /// <summary>
        ///     The start Rcv Data.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool StartRcvData()
        {
            if (IsOpenPort && IsCollecting)
            {
                IsCollecting = true;
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
            if (IsCollecting)
            {
                IsCollecting = false;
                mySerialPort.DataReceived -= PortRcvByteReached;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     The send data.
        /// </summary>
        /// <param name="dataBytes">
        ///     command bytes
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SendData(byte[] dataBytes) {
            try {
                mySerialPort.Write(dataBytes, 0, dataBytes.Length);
                return true;
            }
            catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
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
        public bool SendAngleCommand(int index) {
            byte[] angle = { 0x77, 0x04, 0x00, 0x04, 0x08 };
            angle[2] += (byte)index;
            angle[4] += (byte)index;

            return SendData(angle);
        }

        /// <summary>
        /// auto send angle.
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <param name="index">
        /// the sensor index.
        /// </param>
        /// <param name="period">
        /// send command time period.
        /// </param>
        /// <param name="dual">
        /// send command time dual.
        /// </param>
        public void AutoSendAngle(CancellationToken token, int index, int period = 1000, int dual = 0)
        {
            byte[] angle = { 0x77, 0x04, 0x00, 0x04, 0x08 };
            angle[2] += (byte)index;
            angle[4] += (byte)index;
            Timer timer = new Timer(SendCommand, angle, Timeout.Infinite, period);
            Task.Run(
                () =>
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            timer.Change(0, period);
                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            timer.Dispose();
                            throw;
                        }
                    },
                token);
        }

            void SendCommand(object obj)
            {
                var angle = obj is byte[] ? (byte[])obj : default;

                if (IsOpenPort)
                {
                    try
                    {
                        SendData(angle);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                }
            }



        /// <summary>
        ///     The set port param.
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
        public bool SetPortParam(string portName, int baudRate) {
            if (portName == null || portName.Length <= 0 || baudRate <= 0) {
                return false;
            }
            
            portsNames = SerialPort.GetPortNames();
            if (!TestPortName(portName)) {
                return false;
            }

            mySerialPort = SetPortProperty(portName, baudRate, Parity.None, StopBits.One);
            return true;
        }

        /// <summary>
        ///  The set port property.
        /// </summary>
        /// <param name="portName">
        ///  The port name.
        /// </param>
        /// <param name="baudRate">
        ///  The baud rate.
        /// </param>
        /// <param name="parity">
        ///  The parity.
        /// </param>
        /// <param name="stopBits">
        ///  The stop bits.
        /// </param>
        /// <returns>
        /// The <see cref="SerialPort"/>.
        /// </returns>
        private SerialPort SetPortProperty(string portName, int baudRate, Parity parity, StopBits stopBits)
        {
            if (baudRate <= 0)
            {
                return null;
            }

            return new SerialPort { PortName = portName, BaudRate = baudRate, Parity = parity, StopBits = stopBits };
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
            if (name == null || name.Length <= 0) {
                return false;
            }

            foreach(string s in portsNames) {
                if (s.Equals(name)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     The close port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool ClosePort()
        {
            // serialportIsClosing为true后，mySerialPort_DataReceived就不会在接收数据
            // 等待个20毫秒，以确保不再接收，在关闭串口
            // 否则，如果频繁点击打开/关闭 串口还在接收数据就关闭串口会出现界面卡死
            Thread.Sleep(10);
            mySerialPort.Close();
            return true;
        }

        /// <summary>
        /// The init port.
        /// </summary>
        /// <param name="com">
        /// The com.
        /// </param>
        /// <param name="rate">
        /// The rate.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InitPort(string com = "COM3", int rate = 19200) {
            return SetPortParam(com, rate);
        }

        /// <summary>
        ///     The open port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool OpenPort() {
            try {
                mySerialPort.Open();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
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
            int n = mySerialPort.BytesToRead;
            var buf = new byte[n];
            mySerialPort.Read(buf, 0, n);
            RcvList.AddRange(buf);
            Task t1 = Task.Run(
                               () => {
                                   if (RcvList.Count >= 14) {
                                       lock(rcvLock) {
                                           for(int i = 0; i < RcvList.Count; i++) {
                                               if (RcvList[i] != 0x77) {
                                                   continue;
                                               }
                                               var temp = RcvList.GetRange(i, 14);
                                               RcvList.RemoveRange(i, 14);
                                               RcvCQueue.Enqueue(temp.ToArray());
                                           }
                                       }
                                   }

                                   var result = new byte[14];

                                   RcvCQueue.TryDequeue(out result);
                                   Angles angles = new Angles(0, 0, 0);
                                   GetRcvData(result, out angles);
                                   if (result != null && IsCollecting) {
                                       int index = result[2];
                                       OnSendEventHandler(new SensorEventArgs(angles, index));
                                   }
                               });
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
        void GetRcvData(byte[] srcBytes, out Angles angles) {
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

            Array.Copy(srcBytes, 4, h_bytes, 0, 3);
            Array.Copy(srcBytes, 7, p_bytes, 0, 3);
            Array.Copy(srcBytes, 10, r_bytes, 0, 3);

            double heading = GetDoubleAngle(h_bytes);
            double pitch = GetDoubleAngle(p_bytes);
            double roll = GetDoubleAngle(r_bytes);

            angles = new Angles(heading, pitch, roll);
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
        int GetHeight4(byte data)
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
        int GetLow4(byte data)
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
         double GetDoubleAngle(byte[] srcBytes) {
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
        public List<string> GetPortNames() {
            return new List<string>(SerialPort.GetPortNames());
        }

        /// <summary>
        ///     The on rcv event handler.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected virtual void OnSendEventHandler(SensorEventArgs e) {
            SendEventHandler?.Invoke(this, e);
        }
        #endregion
    }
}
