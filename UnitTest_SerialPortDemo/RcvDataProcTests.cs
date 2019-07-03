// namespace UnitTest_SerialPortDemo
// {
//     using System;
//
//     using Xunit;
//     using SerialPortDemo;
//     using SerialPortDemo.Model;
//
//     public class DataProcUnitTests :IClassFixture<SerialPortComFixture>, IDisposable
//     {
//
//         SerialPortComFixture serialPortComFixture;
//
//         public DataProcUnitTests(SerialPortComFixture serialPortComFixture)
//         {
//             this.serialPortComFixture = serialPortComFixture;
//         }
//
//         public void Dispose()
//         { }
//
//         private DataProcUnit CreateRcvDataProc()
//         {
//             return new DataProcUnit();
//         }
//
//         [Fact]
//         public void StartRcvData_StateUnderTest_ExpectedBehavior()
//         {
//             // Arrange
//             var unitUnderTest = this.CreateRcvDataProc();
//             
//             
//             // Act
//             var result = unitUnderTest.StartRcvData();
//
//             // Assert
//             Assert.True(result);
//         }
//
//         [Fact]
//         public void GetHeight4_StateUnderTest_ExpectedBehavior()
//         {
//             // Arrange
//             var unitUnderTest = this.CreateRcvDataProc();
//             byte data = 0x1f;
//
//             // Act
//             var result = unitUnderTest.GetHeight4(
//                 data);
//
//             // Assert
//             Assert.True(result == 1);
//         }
//
//         [Fact]
//         public void GetLow4_StateUnderTest_ExpectedBehavior()
//         {
//             // Arrange
//             var unitUnderTest = this.CreateRcvDataProc();
//             byte data = 0x1f;
//
//             // Act
//             var result = unitUnderTest.GetLow4(
//                 data);
//
//             // Assert
//             Assert.True(result == 15);
//         }
//
//         [Fact]
//         public void GetDoubleAngle_StateUnderTest_ExpectedBehavior()
//         {
//             // Arrange
//             var unitUnderTest = this.CreateRcvDataProc();
//             byte[] srcBytes = { 0x03, 0x13, 0x71 };
//
//             // Act
//             var result = unitUnderTest.GetDoubleAngle(
//                 srcBytes);
//
//             // Assert
//             Assert.True(result == 313.71);
//         }
//
//         [Fact]
//         public void SendAngleCommand_StateUnderTest_ExpectedBehavior() {
//             // Arrange
//             var unitUnderTest = this.CreateRcvDataProc();
//             unitUnderTest.InitPort();
//             unitUnderTest.OpenPort();
//             int t1 = 0;
//             int t2 = 1;
//             int t3 = 11;
//
//             // Act
//             var result0 = unitUnderTest.SendAngleCommand(t1);
//             var result1 = unitUnderTest.SendAngleCommand(t2);
//             var result2 = unitUnderTest.SendAngleCommand(t3);
//
//             // Assert
//             Assert.True(result0);
//             Assert.True(result1);
//             Assert.True(result2);
//         }
//     }
// }
