// 201906149:03

namespace SerialPortDemo {
    /// <summary>
    /// port command.
    /// </summary>
    public static class PortCommand {
        static PortCommand() {
            GetComReadAngle = "77 04 00 04 08";
        }

        /// <summary>
        /// Gets com read angle.
        /// </summary>
        public static string GetComReadAngle {
            get;
        }
    }
}
