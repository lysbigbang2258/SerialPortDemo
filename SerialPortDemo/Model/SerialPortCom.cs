// 2019060310:29

namespace SerialPortDemo.Model {
    using System;
    using System.IO.Ports;
    using System.Threading;
    using System.Windows;

    /// <summary>
    ///     Manager The serial port com.
    /// </summary>
    public class SerialPortCom {
        /// <summary>
        ///     The All ports name.
        /// </summary>
        string[] portsNames;

        /// <summary>
        ///     The serial port is closing.
        /// </summary>
        bool serialPortIsClosing;

        /// <summary>
        ///     The my serial port resource.
        /// </summary>
        SerialPort mySerialPort;

        /// <summary>
        /// The rcv event handler.
        /// </summary>
        public event EventHandler<byte[]> RcvEventHandler;


        /// <summary>
        ///     Initializes a new instance of the <see cref="SerialPortCom" /> class.
        /// </summary>
        public SerialPortCom() {
            portsNames = null;
            serialPortIsClosing = false;
            mySerialPort = new SerialPort();

        }

        /// <summary>
        ///     Gets a value indicating whether is open.
        /// </summary>
        public bool IsOpen {
            get => mySerialPort.IsOpen;
        }

        /// <summary>
        ///     The get port name.
        /// </summary>
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <returns>
        ///     The result <see cref="bool" />.
        /// </returns>
        public bool TestPortName(string name) {
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
        ///     The set port property.
        /// </summary>
        /// <param name="portName">
        ///     The port Name.
        /// </param>
        /// <param name="baudRate">
        ///     Set The baudRate.
        /// </param>
        /// <param name="parity">
        ///     Set The parity.
        /// </param>
        /// <param name="stopBits">
        ///     Set The stop bits.
        /// </param>
        /// <returns>
        ///     The <see cref="SerialPort" />.
        /// </returns>
        public SerialPort SetPortProperty(string portName, int baudRate, Parity parity, StopBits stopBits) {
            if (baudRate <= 0) {
                throw new ArgumentOutOfRangeException();
            }

            return new SerialPort { PortName = portName, BaudRate = baudRate, Parity = parity, StopBits = stopBits };
        }

        /// <summary>
        ///     opens serial port.
        /// </summary>
        /// <returns>
        ///     IsOpen <see cref="bool" />.
        /// </returns>
        public bool OpenSerialPort() {
            try {
                mySerialPort.Open();
                mySerialPort.DataReceived += MySerialPort_DataReceived;
                
                return true;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        ///     The my serial port_ data received.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void MySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            // 如果串口正在关闭，返回
            if (serialPortIsClosing) {
                return;
            }

            int n = mySerialPort.BytesToRead;
            var buf = new byte[n];
            mySerialPort.Read(buf, 0, n);
            // Todo
            OnRcvEventHandler(buf);
        }

        /// <summary>
        ///     The init serial port.
        /// </summary>
        /// <param name="name">
        ///     The port Name.
        /// </param>
        /// <param name="baudRate">
        ///  The baud rate
        /// </param>
        /// <returns>
        ///  IsInit bool<see cref="bool" />.
        /// </returns>
        public bool InitSerialPort(string name, int baudRate) {
            if (name == null || name.Length <= 0 || baudRate <= 0) {
                return false;
            }
            
            portsNames = SerialPort.GetPortNames();
            if (!TestPortName(name)) {
                return false;
            }

            mySerialPort = SetPortProperty(name, baudRate, Parity.None, StopBits.One);
            return true;
        }

        /// <summary>
        ///     The close serial port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool CloseSerialPort() {
            serialPortIsClosing = true;

            // serialportIsClosing为true后，mySerialPort_DataReceived就不会在接收数据
            // 等待个20毫秒，以确保不再接收，在关闭串口
            // 否则，如果频繁点击打开/关闭 串口还在接收数据就关闭串口会出现界面卡死
            Thread.Sleep(10);
            mySerialPort.Close();
            return true;
        }

        /// <summary>
        ///     The hex string to byte array.
        /// </summary>
        /// <param name="hexStr">
        ///     The hexStr.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public byte[] HexStringToByteArray(string hexStr) {
            string s = hexStr.Replace(" ", string.Empty);

            if (s.Length % 2 != 0) {
                s = s.Substring(0, s.Length - 1) + "0" + s.Substring(s.Length - 1);
            }

            var buffer = new byte[s.Length / 2];

            for(int i = 0; i < s.Length; i += 2) {
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }

        /// <summary>
        ///     The send data to serial port.
        /// </summary>
        /// <param name="str">
        ///     The hex str.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SendData(string str) {
            var bytesdata = HexStringToByteArray(str);

            try {
                mySerialPort.Write(bytesdata, 0, bytesdata.Length);
                return true;
            }
            catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        ///     The send data to serial port.
        /// </summary>
        /// <param name="str">
        ///     The hex str.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SendData(byte[] values) {
            try {
                mySerialPort.Write(values, 0, values.Length);
                return true;
            }
            catch(Exception e) {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// The send angle command.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendAngleCommand() {
            return SendData(PortCommand.GetComReadAngle);
        }


        /// <summary>
        /// The on rcv event handler.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnRcvEventHandler(byte[] e) {
            RcvEventHandler?.Invoke(this, e);
        }

        public string[] GetPortNames() {
            return SerialPort.GetPortNames();
        }
    }
}
