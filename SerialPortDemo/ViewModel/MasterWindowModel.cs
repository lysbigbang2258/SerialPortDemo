// 2019062416:34

namespace SerialPortDemo.ViewModel {
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SerialPortDemo.Model;

    public class MasterWindowModel : ViewModelBase {
        #region Filed

        string filePath;

        string fileName;

        string samplingFreq;

        string timedata;

        string sendNum;

        string rcvNum;

        bool isStartSave;

        ObservableCollection<string> comCollection;

        ObservableCollection<int> baundCollection;

        bool isOpen;

        string rcvRate;

        bool isAutoSave;

        string textMsg;

        public Angles anglesContent;

        RelayCommand openSerial;

        RelayCommand selectedOnComCombox;

        RelayCommand selectedOnBaundCombox;

        RelayCommand changePath;

        RelayCommand changeFileName;

        RelayCommand saveOnceCommand;

        RelayCommand saveLogText;

        RelayCommand clearLogText;

        #endregion

        public MasterWindowModel() {
            DataProc = new DataProcUnit();
            DataProc.SendEventHandler += GetDataReached;
        }

        #region Property

        public DataProcUnit DataProc {
            get;
            set;
        }

        public string FilePath {
            get => filePath;
            set {
                filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        public string FileName {
            get => fileName;

            set {
                fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        public string SamplingFreq {
            get => samplingFreq;

            set {
                samplingFreq = value;
                RaisePropertyChanged(() => SamplingFreq);
            }
        }

        public string TimeData {
            get => timedata;

            set {
                timedata = value;
                RaisePropertyChanged(() => TimeData);
            }
        }

        public string SendNum {
            get => sendNum;

            set {
                filePath = value;
                RaisePropertyChanged(() => SendNum);
            }
        }

        public string RcvNum {
            get => rcvNum;

            set {
                rcvNum = value;
                RaisePropertyChanged(() => RcvNum);
            }
        }

        public bool IsStartSave {
            get => isStartSave;

            set {
                isStartSave = value;
                RaisePropertyChanged(() => IsStartSave);
            }
        }

        public ObservableCollection<string> ComCollection {
            get => comCollection;

            set {
                comCollection = value;
                RaisePropertyChanged(() => ComCollection);
            }
        }

        public ObservableCollection<int> BaundCollection {
            get => baundCollection;

            set {
                baundCollection = value;
                RaisePropertyChanged(() => BaundCollection);
            }
        }

        public bool IsOpen {
            get => isOpen;

            set {
                isOpen = value;
                RaisePropertyChanged(() => IsOpen);
            }
        }

        public string RcvRate {
            get => rcvRate;

            set {
                rcvRate = value;
                RaisePropertyChanged(() => RcvRate);
            }
        }

        public bool IsAutoSave {
            get => isAutoSave;

            set {
                isAutoSave = value;
                RaisePropertyChanged(() => IsAutoSave);
            }
        }

        public string TextMsg {
            get => textMsg;
            set {
                textMsg = value;
                RaisePropertyChanged(() => TextMsg);
            }
        }

        public Angles AnglesContent {
            get => anglesContent;
            set {
                anglesContent = value;
                RaisePropertyChanged(() => AnglesContent);
            }
        }

        RelayCommand OpenSerial {
            get => openSerial ?? (openSerial = new RelayCommand(ExcuteOpenSerial));

            set => openSerial = value;
        }

        RelayCommand SelectedOnComCombox {
            get => selectedOnComCombox ?? (selectedOnComCombox = new RelayCommand(ExcuteSelectedOnComCombox));

            set => selectedOnBaundCombox = value;
        }

        RelayCommand SelectedOnBaundCombox {
            get => selectedOnBaundCombox ?? (selectedOnBaundCombox = new RelayCommand(ExcuteSelectedOnBaundCombox));

            set => selectedOnBaundCombox = value;
        }

        RelayCommand ChangePath {
            get => changePath ?? (changePath = new RelayCommand(ExcuteChangePath));

            set => changePath = value;
        }

        RelayCommand ChangeFileName {
            get => changeFileName ?? (changeFileName = new RelayCommand(ExcuteChangeFileName));

            set => changePath = value;
        }

        RelayCommand SaveOnceCommand {
            get => saveOnceCommand ?? (saveOnceCommand = new RelayCommand(ExcuteSaveOnceCommand));

            set => saveOnceCommand = value;
        }

        RelayCommand SaveLogText {
            get => saveLogText ?? (saveLogText = new RelayCommand(ExcuteSaveLogText));

            set => saveLogText = value;
        }

        RelayCommand ClearLogText {
            get => clearLogText ?? (clearLogText = new RelayCommand(ExcuteClearLogText));

            set => clearLogText = value;
        }

        #endregion

        #region Method

        void ExcuteOpenSerial() {

        }

        void ExcuteSelectedOnComCombox() { }

        void ExcuteSelectedOnBaundCombox() { }

        void ExcuteChangePath() { }

        void ExcuteChangeFileName() { }

        void ExcuteSaveOnceCommand() { }

        void ExcuteSaveLogText() { }

        void ExcuteClearLogText() { }

        void GetDataReached(object sender, SensorEventArgs e) { }

        #endregion
    }

    // #endregion
}
