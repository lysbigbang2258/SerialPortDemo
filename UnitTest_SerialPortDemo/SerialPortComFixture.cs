// 2019061713:56

namespace UnitTest_SerialPortDemo {
    using System;

    using SerialPortDemo;
    using SerialPortDemo.Model;

    public class SerialPortComFixture:IDisposable {

        public readonly SerialPortCom MyPortCom;

        public SerialPortComFixture() {
            MyPortCom = new SerialPortCom();
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose() { }

        #endregion
    }
}
