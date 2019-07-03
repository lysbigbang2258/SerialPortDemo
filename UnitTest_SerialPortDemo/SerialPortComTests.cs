// namespace UnitTest_SerialPortDemo {
//     using System;
//     using System.Collections.Generic;
//     using System.IO.Ports;
//
//     using SerialPortDemo;
//     using SerialPortDemo.Model;
//
//     using Xunit;
//     using Xunit.Sdk;
//
//     /// <summary>
//     ///     The serial port com tests.
//     /// </summary>
//     public class SerialPortComTests : IClassFixture<SerialPortComFixture>, IDisposable {
//         /// <summary>
//         /// The serial port com fixture.
//         /// </summary>
//         readonly SerialPortComFixture serialPortComFixture;
//
//
//         /// <summary>
//         /// Initializes a new instance of the <see cref="SerialPortComTests"/> class.
//         /// </summary>
//         /// <param name="serialPortComFixture">
//         /// The serial port com fixture.
//         /// </param>
//         public SerialPortComTests(SerialPortComFixture serialPortComFixture) {
//             this.serialPortComFixture = serialPortComFixture;
//         }
//
//         /// <summary>
//         /// The dispose.
//         /// </summary>
//         public void Dispose() { }
//
//         /// <summary>
//         /// The create serial port com.
//         /// </summary>
//         /// <returns>
//         /// The <see cref="SerialPortComFixture"/>.
//         /// </returns>
//         DataProcUnit CreateSerialPortCom() {
//             return serialPortComFixture.MyPortCom;
//         }
//
//         [Fact]
//         public void CloseSerialPort_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             var unitUnderTest = CreateSerialPortCom();
//
//             // Act
//             bool result = unitUnderTest.CloseSerialPort();
//
//             // Assert
//             Assert.True(result);
//
//             // Assert.True(!unitUnderTest.IsPortOpen);
//         }
//
//         [Fact]
//         public void GetPortNames_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//
//             // Act
//             var result = unitUnderTest.GetPortNames();
//
//             // Assert
//             Assert.True(result != null);
//         }
//
//         [Fact]
//         public void HexStringToByteArray_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             string hexStr = "77 04 00 04 08";
//             string hexStr1 = "77 04 00 04 8";
//
//             // Act
//             var result = unitUnderTest.HexStringToByteArray(hexStr);
//             var result1 = unitUnderTest.HexStringToByteArray(hexStr1);
//
//             // Assert
//             var cprbytes = new List<byte> { 0x77, 0x04, 0x00, 0x04, 0x08 }.ToArray();
//             for(int i = 0; i < cprbytes.Length; i++) {
//                 Assert.True(result[i] == cprbytes[i]);
//             }
//
//             var cprbytes1 = new List<byte> { 0x77, 0x04, 0x00, 0x04, 0x08 }.ToArray();
//
//             for(int i = 0; i < cprbytes1.Length; i++) {
//                 Assert.True(result1[i] == cprbytes1[i]);
//             }
//
//         }
//
//         [Fact]
//         public void InitSerialPort_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             string name = "COM3";
//             int baudRate = 9600;
//
//             // Act
//             bool result = unitUnderTest.InitSerialPort(name, baudRate);
//
//             bool result1 = unitUnderTest.InitSerialPort(null, baudRate);
//             bool result2 = unitUnderTest.InitSerialPort(name, 0);
//             bool result3 = unitUnderTest.InitSerialPort("Com3", baudRate);
//
//
//             // Assert
//             Assert.True(result);
//             Assert.False(result1);
//             Assert.False(result2);
//             Assert.False(result3);
//         }
//
//
//         [Fact]
//         public void OpenSerialPort_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             SerialPortCom unitUnderTest1 = CreateSerialPortCom();
//
//             // Act
//             string name = "COM3";
//             int baudRate = 19200;
//             unitUnderTest.InitSerialPort(name, baudRate);
//             bool result = unitUnderTest.OpenSerialPort();
//             
//             unitUnderTest1.InitSerialPort(name, baudRate);
//             
//             bool result1 = unitUnderTest1.OpenSerialPort();
//             
//             // Assert
//             Assert.True(result);
//             Assert.False(result1);
//         }
//
//
//         [Fact]
//         public void SendAngleCommand_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             string name = "COM3";
//             int baudRate = 19200;
//             bool result0 = unitUnderTest.InitSerialPort(name, baudRate);
//             bool result1 = unitUnderTest.OpenSerialPort();
//             // Act
//             bool result = unitUnderTest.SendAngleCommand();
//
//             // Assert
//             Assert.True(result);
//             Assert.True(result0);
//             Assert.True(result1);
//         }
//
//         [Fact]
//         public void SendData_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             string str = "77 04 01 04 09";
//             string name = "COM3";
//             int baudRate = 19200;
//             unitUnderTest.InitSerialPort(name, baudRate);
//             unitUnderTest.OpenSerialPort();
//
//             // Act
//             bool result = unitUnderTest.SendData(str);
//
//             unitUnderTest.CloseSerialPort();
//             bool result1 = unitUnderTest.SendData(str);
//
//             // Assert
//             Assert.True(result);
//             Assert.False(result1);
//
//         }
//
//         [Fact]
//         public void SetPortProperty_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             string portName = "COM3";
//             int baudRate = 19200;
//             Parity parity = Parity.None;
//             StopBits stopBits = StopBits.One;
//
//             // Act
//             SerialPort result = unitUnderTest.SetPortProperty(portName, baudRate, parity, stopBits);
//
//             // Assert
//             Assert.True(result.PortName == portName);
//             Assert.True(result.BaudRate == baudRate);
//             Assert.True(result.Parity == parity);
//             Assert.True(result.StopBits == stopBits);
//
//             // Act
//            
//
//             Assert.Throws<ArgumentOutOfRangeException>(() => unitUnderTest.SetPortProperty(portName, 0, parity, stopBits));
//         }
//
//
//         [Fact]
//         public void TestPortName_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             SerialPortCom unitUnderTest = CreateSerialPortCom();
//             string name = "COM3";
//             int baudRate = 9600;
//             unitUnderTest.InitSerialPort(name, baudRate);
//             unitUnderTest.OpenSerialPort();
//
//             // Act
//             bool result = unitUnderTest.TestPortName(name);
//             bool result1 = unitUnderTest.TestPortName(null);
//             bool result2 = unitUnderTest.TestPortName("COM4");
//
//
//             // Assert
//             Assert.True(result);
//             Assert.False(result1);
//             Assert.False(result2);
//         }
//     }
// }
