// 2019071916:10

namespace SerialPortDemo.Model
{
    using System;

    /// <summary>
    /// The save event args.
    /// </summary>
    public class SaveEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SaveEventArgs"/> class.
        /// </summary>
        /// <param name="num">
        /// The Sensor num.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public SaveEventArgs(int num, string data)
        {
            Data = data;
            Num = num;
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the num.
        /// </summary>
        public int Num {
            get;
            set;
        }
    }
}