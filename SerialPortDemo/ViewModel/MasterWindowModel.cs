// 2019062416:34

namespace SerialPortDemo.ViewModel
{
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using SerialPortDemo.Model;
    using SerialPortDemo.View;

    /// <summary>
    /// The master window model.
    /// </summary>
    public class MasterWindowModel : ViewModelBase
    {
        /// <summary>
        /// The angles content.
        /// </summary>
        Angles anglesContent;

        /// <summary>
        /// The baud collection binding com-box.
        /// </summary>
        ObservableCollection<int> baudCollection;

        /// <summary>
        /// The baud collection item is Selected.
        /// </summary>
        int baudCollectionItem;

        /// <summary>
        /// The command changing file name.
        /// </summary>
        RelayCommand changeFileNameCommand;

        /// <summary>
        /// The command of changing path.
        /// </summary>
        RelayCommand changePathCommand;

        /// <summary>
        /// The command of clear log text.
        /// </summary>
        RelayCommand clearLogTextCommand;

        /// <summary>
        /// The command of collecting data.
        /// </summary>
        RelayCommand collectDataCommand;

        /// <summary>
        /// The Com collection binding com-box.
        /// </summary>
        ObservableCollection<string> comCollection;

        /// <summary>
        ///  The com collection item is Selected.
        /// </summary>
        string comCollectionItem;

        /// <summary>
        /// The command of saving once  data command.
        /// </summary>
        RelayCommand coolNSaveCommand;

        /// <summary>
        /// Now The current time data.
        /// </summary>
        string curTime;

        /// <summary>
        /// The saved file name.
        /// </summary>
        string fileName;

        /// <summary>
        /// The saved file path.
        /// </summary>
        string filePath;

        /// <summary>
        /// The auto save is start or not.
        /// </summary>
        bool isAutoSave;

        /// <summary>
        /// The serial port is collected data.
        /// </summary>
        bool isCollected;

        /// <summary>
        /// The port is open or not.
        /// </summary>
        bool isOpenPort;

        /// <summary>
        /// The command of opening serial .
        /// </summary>
        RelayCommand openSerialCommand;

        /// <summary>
        /// The proc unit object.
        /// </summary>
        DataProcUnit procUnit;

        /// <summary>
        /// The rcv Packets.
        /// </summary>
        string rcvPackets;

        /// <summary>
        /// The rcv data rate.
        /// </summary>
        string rcvRate;

        /// <summary>
        /// The sampling frequency.
        /// </summary>
        string samplingFreq;

        /// <summary>
        /// The save file frequency.
        /// </summary>
        string saveFreq;

        /// <summary>
        /// The command of logging text.
        /// </summary>
        RelayCommand saveLogTextCommand;

        /// <summary>
        /// The command of selecting on baud com-box.
        /// </summary>
        RelayCommand selectOnBaudComboxCommand;

        /// <summary>
        /// The command of selecting com com-box.
        /// </summary>
        RelayCommand selectOnComComboxCommand;

        /// <summary>
        /// The send  packets.
        /// </summary>
        string sendPackets;

        /// <summary>
        /// The log or command text msg.
        /// </summary>
        string textMsg;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterWindowModel"/> class.
        /// </summary>
        public MasterWindowModel()
        {
            ProcUnit.SendEventHandler += GetDataReached;
            Init();
        }

        /// <summary>
        /// Gets or sets the angles content.
        /// </summary>
        public Angles AnglesContent {
            get => anglesContent;
            set {
                anglesContent = value;
                RaisePropertyChanged(() => AnglesContent);
            }
        }

        /// <summary>
        /// Gets or sets the baud collection.
        /// </summary>
        public ObservableCollection<int> BaudCollection {
            get => baudCollection;

            set {
                baudCollection = value;
                RaisePropertyChanged(() => BaudCollection);
            }
        }

        /// <summary>
        /// Gets or sets the baud collection item.
        /// </summary>
        public int BaudCollectionItem {
            get => baudCollectionItem;
            set {
                baudCollectionItem = value;
                RaisePropertyChanged(() => BaudCollectionItem);
            }
        }

        /// <summary>
        /// Gets or sets the change file name.
        /// </summary>
        public RelayCommand ChangeFileNameCommand {
            get => changeFileNameCommand ?? (changeFileNameCommand = new RelayCommand(execute: ExcuteChangeFileName));

            set => changePathCommand = value;
        }

        /// <summary>
        /// Gets or sets the change path.
        /// </summary>
        public RelayCommand ChangePathCommand {
            get => changePathCommand ?? (changePathCommand = new RelayCommand(execute: ExcuteChangePath));

            set => changePathCommand = value;
        }

        /// <summary>
        /// Gets or sets the clear log text.
        /// </summary>
        public RelayCommand ClearLogTextCommand {
            get => clearLogTextCommand ?? (clearLogTextCommand = new RelayCommand(execute: ExcuteClearLogText));

            set => clearLogTextCommand = value;
        }

        /// <summary>
        /// Gets or sets the collect data.
        /// </summary>
        public RelayCommand CollectDataCommand {
            get => collectDataCommand ?? (collectDataCommand = new RelayCommand(ExcuteCollectData));
            set {
                collectDataCommand = value;
            }
        }

        /// <summary>
        /// Gets or sets the com collection.
        /// </summary>
        public ObservableCollection<string> ComCollection {
            get => comCollection;

            set {
                comCollection = value;
                RaisePropertyChanged(() => ComCollection);
            }
        }

        /// <summary>
        /// Gets or sets the com collection item.
        /// </summary>
        public string ComCollectionItem {
            get => comCollectionItem;
            set {
                comCollectionItem = value;
                RaisePropertyChanged(() => ComCollectionItem);
            }
        }

        /// <summary>
        /// Gets or sets the cur time.
        /// </summary>
        public string CurTime {
            get => curTime;

            set {
                curTime = value;
                RaisePropertyChanged(() => CurTime);
            }
        }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName {
            get => fileName;

            set {
                fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath {
            get => filePath;
            set {
                filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is auto save.
        /// </summary>
        public bool IsAutoSave {
            get => isAutoSave;

            set {
                isAutoSave = value;
                RaisePropertyChanged(() => IsAutoSave);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is collected.
        /// </summary>
        public bool IsCollected {
            get => isCollected;

            set {
                isCollected = value;
                RaisePropertyChanged(() => IsCollected);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is open.
        /// </summary>
        public bool IsOpen {
            get => isOpenPort;

            set {
                isOpenPort = value;
                RaisePropertyChanged(() => IsOpen);
            }
        }

        /// <summary>
        /// Gets or sets the open serial.
        /// </summary>
        public RelayCommand OpenSerialCommand {
            get => openSerialCommand ?? (openSerialCommand = new RelayCommand(execute: ExcuteOpenSerial));

            set => openSerialCommand = value;
        }

        /// <summary>
        /// Gets or sets the proc unit.
        /// </summary>
        public DataProcUnit ProcUnit {
            get => procUnit ?? (procUnit = new DataProcUnit());
            set => procUnit = value;
        }

        /// <summary>
        /// Gets or sets the rcv packets.
        /// </summary>
        public string RcvPackets {
            get => rcvPackets;

            set {
                rcvPackets = value;
                RaisePropertyChanged(() => RcvPackets);
            }
        }

        /// <summary>
        /// Gets or sets the rcv rate.
        /// </summary>
        public string RcvRate {
            get => rcvRate;

            set {
                rcvRate = value;
                RaisePropertyChanged(() => RcvRate);
            }
        }

        /// <summary>
        /// Gets or sets the sampling freq.
        /// </summary>
        public string SamplingFreq {
            get => samplingFreq;

            set {
                samplingFreq = value;
                RaisePropertyChanged(() => SamplingFreq);
            }
        }

        /// <summary>
        /// Gets or sets the save freq.
        /// </summary>
        public string SaveFreq {
            get => saveFreq;
            set {
                saveFreq = value;
                RaisePropertyChanged(() => SaveFreq);
            }
        }

        /// <summary>
        /// Gets or sets the save log text.
        /// </summary>
        public RelayCommand SaveLogTextCommand {
            get => saveLogTextCommand ?? (saveLogTextCommand = new RelayCommand(execute: ExcuteSaveLogText));

            set => saveLogTextCommand = value;
        }

        /// <summary>
        /// Gets or sets the save once command.
        /// </summary>
        public RelayCommand SaveOnceCommand {
            get => coolNSaveCommand ?? (coolNSaveCommand = new RelayCommand(execute: ExcuteSaveOnceCommand));

            set => coolNSaveCommand = value;
        }

        /// <summary>
        /// Gets or sets the selected on baud combox command.
        /// </summary>
        public RelayCommand SelectedOnBaudComboxCommand {
            get => selectOnBaudComboxCommand ?? (selectOnBaudComboxCommand = new RelayCommand(execute: ExcuteSelectedOnBaundCombox));

            set => selectOnBaudComboxCommand = value;
        }

        /// <summary>
        /// Gets or sets the select on com combox command.
        /// </summary>
        public RelayCommand SelectOnComComboxCommand {
            get => selectOnComComboxCommand ?? (selectOnComComboxCommand = new RelayCommand(execute: ExcuteSelectedOnComCombox));

            set => selectOnComComboxCommand = value;
        }

        /// <summary>
        /// Gets or sets the send packets.
        /// </summary>
        public string SendPackets {
            get => sendPackets;

            set {
                sendPackets = value;
                RaisePropertyChanged(() => SendPackets);
            }
        }

        /// <summary>
        /// Gets or sets the sensor panel view models.
        /// </summary>
        public ObservableCollection<SensorPanelViewModel> SensorPanelViewModels { get; set; }

        /// <summary>
        /// Gets or sets the sensor panel views.
        /// </summary>
        public ObservableCollection<SensorPanelView> SensorPanelViews { get; set; }

        /// <summary>
        /// Gets or sets the text msg.
        /// </summary>
        public string TextMsg {
            get => textMsg;
            set {
                textMsg = value;
                RaisePropertyChanged(() => TextMsg);
            }
        }

        /// <summary>
        /// TODO The excute change file name.
        /// </summary>
        void ExcuteChangeFileName()
        {
        }

        /// <summary>
        /// TODO The excute change path.
        /// </summary>
        void ExcuteChangePath()
        {
        }

        /// <summary>
        /// TODO The excute clear log text.
        /// </summary>
        void ExcuteClearLogText()
        {
        }

        /// <summary>
        /// The excute collect data.
        /// </summary>
        void ExcuteCollectData()
        {
            ProcUnit.StartRcvData();
            ProcUnit.AutoSendAngle(2);
        }

        /// <summary>
        /// The excute open serial.
        /// </summary>
        void ExcuteOpenSerial()
        {
            if (!IsOpen)
            {
                ProcUnit.InitPort(ComCollectionItem, BaudCollectionItem);
                IsOpen = ProcUnit.OpenPort();
            }
            else
            {
                IsOpen = false;
                ProcUnit.ClosePort();
            }
        }

        /// <summary>
        /// TODO The excute save log text.
        /// </summary>
        void ExcuteSaveLogText()
        {
        }

        /// <summary>
        /// TODO The excute save once command.
        /// </summary>
        void ExcuteSaveOnceCommand()
        {
        }

        /// <summary>
        /// TODO The excute selected on baund combox.
        /// </summary>
        void ExcuteSelectedOnBaundCombox()
        {
        }

        /// <summary>
        /// TODO The excute selected on com combox.
        /// </summary>
        void ExcuteSelectedOnComCombox()
        {
        }

        /// <summary>
        /// The get data reached.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        void GetDataReached(object sender, SensorEventArgs e)
        {
            var num = e.Num;
            SensorPanelViewModels[num].TextHead = e.Angles.Head.ToString();
            SensorPanelViewModels[num].TextPitch = e.Angles.Pitch.ToString();
            SensorPanelViewModels[num].TextRoll = e.Angles.Roll.ToString();
        }

        /// <summary>
        /// The init data.
        /// </summary>
        void Init()
        {
            var t = ProcUnit.GetPortNames();
            ComCollection = new ObservableCollection<string>();
            foreach (string s in t)
            {
                ComCollection.Add(item: s);
            }

            BaudCollection = new ObservableCollection<int> { 2400, 4800, 9600, 19200, 38400, 38400, 57600, 115200 };

            SensorPanelViews = new ObservableCollection<SensorPanelView>();
            SensorPanelViewModels = new ObservableCollection<SensorPanelViewModel>();

            for (int i = 0; i < 8; i++)
            {
                string str = i.ToString();
                SensorPanelViewModel viewModel = new SensorPanelViewModel(num: str, head: str, pitch: str, roll: str);
                SensorPanelView view = new SensorPanelView(viewModel: viewModel);
                SensorPanelViews.Add(item: view);
                SensorPanelViewModels.Add(item: viewModel);
            }
        }
    }

    // #endregion
}