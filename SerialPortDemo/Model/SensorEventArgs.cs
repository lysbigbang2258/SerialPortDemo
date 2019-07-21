// 2019061911:01

namespace SerialPortDemo.Model {
    using System;

    /// <summary>
    /// The sensor event args.
    /// </summary>
    public class SensorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SensorEventArgs"/> class.
        /// </summary>
        /// <param name="angles">
        /// The angles.
        /// </param>
        /// <param name="num">
        /// The num.
        /// </param>
        public SensorEventArgs(Angles angles, int num)
        {
            Angles = angles;
            Num = num;
        }

        /// <summary>
        /// Gets or sets the angles.
        /// </summary>
        public Angles Angles {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the num.
        /// </summary>
        public int Num {
            get;
            set;
        }
    }
}
