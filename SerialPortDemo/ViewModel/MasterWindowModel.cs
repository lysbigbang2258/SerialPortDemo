// 2019062416:34

namespace SerialPortDemo.ViewModel {
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SerialPortDemo.Model;
    using SerialPortDemo.View;

    public class MasterWindowModel : ViewModelBase {
        #region Filed

        #region Property Filed

        string filePath;

        string fileName;

        string samplingFreq;

        string timedata;

        string sendNum;

        string rcvNum;

        bool isStartSave;

        DataProcUnit procUnit;

        ObservableCollection<string> comCollection;

        string comCollectionItem;

        ObservableCollection<int> baundCollection;

        int baundCollectionItem;

        bool isOpen;

        string rcvRate;

        bool isAutoSave;

        string textMsg;

        Angles anglesContent;

        bool isCollected;

        #endregion

        #region Command Filed

        RelayCommand openSerial;

        RelayCommand selectedOnComCombox;

        RelayCommand selectedOnBaundCombox;

        RelayCommand changePath;

        RelayCommand changeFileName;

        RelayCommand saveOnceCommand;

        RelayCommand saveLogText;

        RelayCommand clearLogText;

        RelayCommand collectData;



        #endregion

        #endregion

        #region Init

        public MasterWindowModel() {
            ProcUnit.SendEventHandler += GetDataReached;
            Init();
        }

        void Init() {
            var t = ProcUnit.GetPortNames();
            ComCollection = new ObservableCollection<string>();
            foreach(string s in t) {
                ComCollection.Add(item: s);
            }

            BaundCollection = new ObservableCollection<int> { 2400, 4800, 9600, 19200, 38400, 38400, 57600, 115200 };


            SensorPanelViews = new ObservableCollection<SensorPanelView>();
            SensorPanelViewModels = new ObservableCollection<SensorPanelViewModel>();

            for(int i = 0; i < 8; i++) {
                string str = i.ToString();
                SensorPanelViewModel viewModel = new SensorPanelViewModel(num: str, head: str, pitch: str, roll: str);
                SensorPanelView view = new SensorPanelView(viewModel: viewModel);
                SensorPanelViews.Add(item: view);
                SensorPanelViewModels.Add(item: viewModel);
            }
        }

        #endregion

        #region Property

        #region Field Property

        public ObservableCollection<SensorPanelView> SensorPanelViews {
            get;
            set;
        }

        public ObservableCollection<SensorPanelViewModel> SensorPanelViewModels {
            get;
            set;
        }

        public DataProcUnit ProcUnit {
            get => procUnit ?? (procUnit = new DataProcUnit());
            set => procUnit = value;
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

        public bool IsCollected {
            get => isCollected;

            set {
                isCollected = value;
                RaisePropertyChanged(() => IsCollected);
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

        public string ComCollectionItem {
            get => comCollectionItem;
            set {
                comCollectionItem = value;
                RaisePropertyChanged(() => ComCollectionItem);
            }
        }

        public int BaundCollectionItem {
            get => baundCollectionItem;
            set {
                baundCollectionItem = value;
                RaisePropertyChanged(() => BaundCollectionItem);
            }
        }


        #endregion

        #region Command Property

        public RelayCommand OpenSerial {
            get => openSerial ?? (openSerial = new RelayCommand(execute: ExcuteOpenSerial));

            set => openSerial = value;
        }

        public RelayCommand SelectedOnComCombox {
            get => selectedOnComCombox ?? (selectedOnComCombox = new RelayCommand(execute: ExcuteSelectedOnComCombox));

            set => selectedOnBaundCombox = value;
        }

        public RelayCommand SelectedOnBaundCombox {
            get => selectedOnBaundCombox ?? (selectedOnBaundCombox = new RelayCommand(execute: ExcuteSelectedOnBaundCombox));

            set => selectedOnBaundCombox = value;
        }

        public RelayCommand ChangePath {
            get => changePath ?? (changePath = new RelayCommand(execute: ExcuteChangePath));

            set => changePath = value;
        }

        public RelayCommand ChangeFileName {
            get => changeFileName ?? (changeFileName = new RelayCommand(execute: ExcuteChangeFileName));

            set => changePath = value;
        }

        public RelayCommand SaveOnceCommand {
            get => saveOnceCommand ?? (saveOnceCommand = new RelayCommand(execute: ExcuteSaveOnceCommand));

            set => saveOnceCommand = value;
        }

        public RelayCommand SaveLogText {
            get => saveLogText ?? (saveLogText = new RelayCommand(execute: ExcuteSaveLogText));

            set => saveLogText = value;
        }

        public RelayCommand ClearLogText {
            get => clearLogText ?? (clearLogText = new RelayCommand(execute: ExcuteClearLogText));

            set => clearLogText = value;
        }

        public RelayCommand CollectData {
            get => collectData ?? (collectData = new RelayCommand(ExcuteCollectData));
            set {
                collectData = value;
            }
        }


        #endregion

        #endregion

        #region Method

        void ExcuteOpenSerial() {
            if (!IsOpen) {
                ProcUnit.InitPort(ComCollectionItem, BaundCollectionItem);
                IsOpen = ProcUnit.OpenPort();
            }
            else {
                IsOpen = false;
                ProcUnit.ClosePort();
            }
        }

        void ExcuteCollectData() {
            ProcUnit.StartRcvData();
            ProcUnit.AutoSendAngle(2);
        }

        void GetDataReached(object sender, SensorEventArgs e) {
            var num = e.Num;
            SensorPanelViewModels[num].TextHead = e.Angles.Head.ToString();
            SensorPanelViewModels[num].TextPitch = e.Angles.Pitch.ToString();
            SensorPanelViewModels[num].TextRoll = e.Angles.Roll.ToString();
        }

        void ExcuteSelectedOnComCombox() { }

        void ExcuteSelectedOnBaundCombox() { }

        void ExcuteChangePath() { }

        void ExcuteChangeFileName() { }

        void ExcuteSaveOnceCommand() { }

        void ExcuteSaveLogText() { }

        void ExcuteClearLogText() { }

        

        #endregion
    }

    // #endregion
}
