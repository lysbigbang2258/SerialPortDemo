// 2019062615:16

namespace SerialPortDemo.ViewModel {
    using GalaSoft.MvvmLight;

    public class SensorPanelViewModel:ViewModelBase{
        string textHead;

        string textPitch;

        string textRoll;

        string labNum;

        public SensorPanelViewModel(string num, string head, string pitch, string roll) {
            LabNum = num;
            TextHead = head;
            TextRoll = roll;
            TextPitch = pitch;
        }

        public string TextHead {
            get => textHead;
            set {
                textHead = value;
                RaisePropertyChanged(() => TextHead);
            }
        }

        public string TextPitch {
            get => textPitch;
            set {
                textPitch = value;
                RaisePropertyChanged(() => TextPitch);
            }
        }

        public string TextRoll {
            get => textRoll;
            set {
                textRoll = value;
                RaisePropertyChanged(() => TextRoll);
            }
        }

        public string LabNum {
            get => labNum;
            set {
                labNum = value;
                RaisePropertyChanged(() => LabNum);
            }
        }
    }
}
