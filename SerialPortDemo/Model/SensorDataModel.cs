// 2019062014:37

namespace SerialPortDemo.Model {
    using GalaSoft.MvvmLight;

    /// <summary>
    /// The sensor data model.
    /// </summary>
    public class SensorDataModel : ObservableObject {

        string head;

        string pitch;

        string roll;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SensorDataModel" /> class.
        /// </summary>
        public SensorDataModel() {
          
        }


        public string Head {
            get {

                return head;
            }
            set {
                head = value;
                RaisePropertyChanged(() => Head);
            }
        }

        public string Pitch {
            get {
                return pitch;
            }
            set {
                pitch = value;
                RaisePropertyChanged(() => Pitch);

            }
        }

        public string Roll {
            get {
                return roll;
            }
            set{
                roll = value;
                RaisePropertyChanged(() => Roll);

            }
        }
    }
}
