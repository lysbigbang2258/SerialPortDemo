// 2019061415:27

namespace SerialPortDemo.Model {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     The rcv data proc.
    /// </summary>
    public class DataProcUnit {
        #region Filed  Init  Property

        /// <summary>
        ///     The rcv data lock.
        /// </summary>
        readonly object rcvLock = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataProcUnit" /> class.
        /// </summary>
        public DataProcUnit() {
            RcvCQueue = new ConcurrentQueue<byte[]>();
            RcvList = new List<byte>();
            PortCom = new SerialPortCom();
        }

        /// <summary>
        ///     Gets or sets the PortCom.
        /// </summary>
        SerialPortCom PortCom {
            get;
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

        /// <summary>
        ///     The rcv event handler.
        /// </summary>
        public event EventHandler<SensorEventArgs> SendEventHandler;

        #endregion

        #region Method

        /// <summary>
        ///     The start Rcv DataTime.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool StartRcvData() {
            PortCom.RcvEventHandler += PortRcvByteReached;
            return true;
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
            return PortCom.SendData(dataBytes);
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
        ///     auto send angle.
        /// </summary>
        /// <param name="index">
        ///     the sensor index.
        /// </param>
        /// <param name="dual">
        ///     send command time dual.
        /// </param>
        /// <param name="period">
        ///     send command time period.
        /// </param>
        public void AutoSendAngle(int index, int period = 1000, int dual = 0) {
            byte[] angle = { 0x77, 0x04, 0x00, 0x04, 0x08 };
            angle[2] += (byte)index;
            angle[4] += (byte)index;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            int maxcount = 10;
            int invokecount = 0;
            Task.Run(
                     () => {
                         Timer timer = new Timer(
                                 s => {
                                     AutoResetEvent autoReset = (AutoResetEvent)s;
                                     SendData(angle);
                                     invokecount++;
                                     if (invokecount == maxcount) {
                                         autoReset.Set();
                                     }
                                 },
                                 autoEvent,
                                 dual,
                                 period);
                         autoEvent.WaitOne();
                         timer.Dispose();
                     });
        }

        /// <summary>
        ///     The set port param.
        /// </summary>
        /// <param name="portname">
        ///     The portname.
        /// </param>
        /// <param name="boundrate">
        ///     The boundrate.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool SetPortParam(string portname, int boundrate) {
            return PortCom.InitSerialPort(portname, boundrate);
        }

        /// <summary>
        ///     The close port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool ClosePort() {
            return PortCom.CloseSerialPort();
        }

        /// <summary>
        ///     The init port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool InitPort() {
            return SetPortParam("COM3", 19200);
        }

        /// <summary>
        ///     The open port.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public bool OpenPort() {
            return PortCom.OpenSerialPort();
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
        void PortRcvByteReached(object sender, byte[] e) {
            RcvList.AddRange(e);
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
                                   int index = result[2];
                                   OnSendEventHandler(new SensorEventArgs(angles, index));
                               });
        }

        /// <summary>
        ///     The get rcv data.
        /// </summary>
        /// <param name="srcBytes">
        ///     The src bytes.
        /// </param>
        /// <param name="angles">
        ///     The angles.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool GetRcvData(byte[] srcBytes, out Angles angles) {
            if (srcBytes.Length != 14) {
                angles = new Angles(0, 0, 0);
                return false;
            }

            if (srcBytes[0] != 0x77 || srcBytes[1] != 0x0d || srcBytes[3] != 0x84) {
                angles = new Angles(0, 0, 0);
                return false;
            }

            var hBytes = new byte[3];
            var pBytes = new byte[3];
            var rBytes = new byte[3];

            Array.Copy(srcBytes, 4, hBytes, 0, 3);
            Array.Copy(srcBytes, 7, pBytes, 0, 3);
            Array.Copy(srcBytes, 10, rBytes, 0, 3);

            double heading, pitch, roll;

            heading = GetDoubleAngle(hBytes);
            pitch = GetDoubleAngle(pBytes);
            roll = GetDoubleAngle(rBytes);

            angles = new Angles(heading, pitch, roll);
            return true;
        }

        /// <summary>
        ///     The get height 4.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        public int GetHeight4(byte data) {
            // 获取高四位
            int height;
            height = (data & 0xf0) >> 4;
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
        public int GetLow4(byte data) {
            // 获取低四位
            int low;
            low = data & 0x0f;
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
        public double GetDoubleAngle(byte[] srcBytes) {
            double result;

            double sign;

            sign = GetHeight4(srcBytes[0]) == 0 ? 1 : -1;

            result = sign
                     * (100 * GetLow4(srcBytes[0]) + 10 * GetHeight4(srcBytes[1]) + 1 * GetLow4(srcBytes[1]) + 0.1 * GetHeight4(srcBytes[2])
                        + 0.01 * GetLow4(srcBytes[2]));

            return result;
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
