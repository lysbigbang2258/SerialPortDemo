// 
namespace SerialPortDemo.Model {
    /// <summary>
    ///     The angles.
    /// </summary>
    public struct Angles {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Angles" /> struct.
        /// </summary>
        /// <param name="head">
        ///     The head.
        /// </param>
        /// <param name="pitch">
        ///     The pitch.
        /// </param>
        /// <param name="roll">
        ///     The roll.
        /// </param>
        public Angles(double head, double pitch, double roll) {
            Head = head;
            Pitch = pitch;
            Roll = roll;
        }

        /// <summary>
        ///     Gets the head.
        /// </summary>
        public double Head {
            get;
            set;
        }

        /// <summary>
        ///     Gets the pitch.
        /// </summary>
        public double Pitch {
            get;
            set;
        }

        /// <summary>
        ///     Gets the roll.
        /// </summary>
        public double Roll {
            get;
            set;
        }
    }
}
