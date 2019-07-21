// 2019062014:37

namespace SerialPortDemo.Model {
    using GalaSoft.MvvmLight;

    /// <summary>
    /// The sensor data model.
    /// </summary>
    public class SensorDataModel : ObservableObject {
        /// <summary>
        /// The head.
        /// </summary>
        string head;

        /// <summary>
        /// The pitch.
        /// </summary>
        string pitch;

        /// <summary>
        /// The roll.
        /// </summary>
        string roll;

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        public string Head {
            get {
                return head;
            }

            set {
                head = value;
                RaisePropertyChanged(() => Head);
            }
        }

        /// <summary>
        /// Gets or sets the pitch.
        /// </summary>
        public string Pitch {
            get {
                return pitch;
            }

            set {
                pitch = value;
                RaisePropertyChanged(() => Pitch);
            }
        }

        /// <summary>
        /// Gets or sets the roll.
        /// </summary>
        public string Roll {
            get {
                return roll;
            }

            set {
                roll = value;
                RaisePropertyChanged(() => Roll);
            }
        }
    }
}
