// 2019062416:34

namespace SerialPortDemo.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SerialPortDemo.Model;
    using SerialPortDemo.View;

    /// <summary>
    /// The master window model.
    /// </summary>
    public class MasterWindowModel : ViewModelBase
    {
        #region Field

        /// <summary>
        /// The angles content.
        /// </summary>
        private Angles anglesContent;

        /// <summary>
        /// The baud collection binding com-box.
        /// </summary>
        private ObservableCollection<int> baudCollection;

        /// <summary>
        /// The baud collection item is Selected.
        /// </summary>
        private int baudCollectionItem;

        /// <summary>
        /// The command changing file name.
        /// </summary>
        private RelayCommand changeFileNameCommand;

        /// <summary>
        /// The command of changing path.
        /// </summary>
        private RelayCommand changePathCommand;

        /// <summary>
        /// The command of clear log text.
        /// </summary>
        private RelayCommand clearLogTextCommand;

        /// <summary>
        /// The command of collecting data.
        /// </summary>
        private RelayCommand collectDataCommand;

        /// <summary>
        /// The Com collection binding com-box.
        /// </summary>
        private ObservableCollection<string> comCollection;

        /// <summary>
        ///  The com collection item is Selected.
        /// </summary>
        private string comCollectionItem;

        /// <summary>
        /// The command of saving once  data command.
        /// </summary>
        private RelayCommand coolNSaveCommand;

        /// <summary>
        ///  The Cancellation token.
        /// </summary>
        CancellationTokenSource ctTokenSource;

        /// <summary>
        /// Now The current time data.
        /// </summary>
        private string curTime;

        /// <summary>
        /// The saved file name.
        /// </summary>
        private string fileName;

        /// <summary>
        /// The saved file path.
        /// </summary>
        private string filePath;

        /// <summary>
        /// The auto save is start or not.
        /// </summary>
        private bool isAutoSave;

        /// <summary>
        /// The serial port is collected data.
        /// </summary>
        private bool isCollecting;

        /// <summary>
        /// The port is open or not.
        /// </summary>
        private bool isOpenPort;

        /// <summary>
        /// The command of opening serial .
        /// </summary>
        private RelayCommand openSerialCommand;

        /// <summary>
        /// The proc unit object.
        /// </summary>
        private DataProcUnit procUnit;

        /// <summary>
        /// The rcv Packets.
        /// </summary>
        private string rcvPackets;

        /// <summary>
        /// The rcv data rate.
        /// </summary>
        private string rcvRate;

        /// <summary>
        ///  The receive Sensor data string builder 
        /// </summary>
        private StringBuilder rcvStrBuilder;

        /// <summary>
        /// The sampling frequency.
        /// </summary>
        private string samplingFreq;

        /// <summary>
        /// The save file frequency.
        /// </summary>
        private string saveFreq;

        /// <summary>
        /// The command of logging text.
        /// </summary>
        private RelayCommand saveLogTextCommand;

        /// <summary>
        /// The command of selecting on baud com-box.
        /// </summary>
        private RelayCommand selectOnBaudComboxCommand;

        /// <summary>
        /// The command of selecting com com-box.
        /// </summary>
        private RelayCommand selectOnComComboxCommand;

        /// <summary>
        /// The send  packets.
        /// </summary>
        private string sendPackets;

        /// <summary>
        /// The log or command text msg.
        /// </summary>
        private string textMsg;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterWindowModel"/> class.
        /// </summary>
        public MasterWindowModel()
        {
            rcvStrBuilder = new StringBuilder();
            Init();
            ProcUnit.SendEventHandler += GetDataReached;
        }

        #region Property

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
            get => changeFileNameCommand ?? (changeFileNameCommand = new RelayCommand(execute: ExecuteChangeFileName));

            set => changePathCommand = value;
        }

        /// <summary>
        /// Gets or sets the change path.
        /// </summary>
        public RelayCommand ChangePathCommand {
            get => changePathCommand ?? (changePathCommand = new RelayCommand(execute: ExecuteChangePath));

            set => changePathCommand = value;
        }

        /// <summary>
        /// Gets or sets the clear log text.
        /// </summary>
        public RelayCommand ClearLogTextCommand {
            get => clearLogTextCommand ?? (clearLogTextCommand = new RelayCommand(execute: ExecuteClearLogText));

            set => clearLogTextCommand = value;
        }

        /// <summary>
        /// Gets or sets the collect data.
        /// </summary>
        public RelayCommand CollectDataCommand {
            get => collectDataCommand ?? (collectDataCommand = new RelayCommand(execute: ExecuteCollectData));
            set => collectDataCommand = value;
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
        public bool IsCollecting {
            get => isCollecting;

            set {
                isCollecting = value;
                RaisePropertyChanged(() => IsCollecting);
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
            get => openSerialCommand ?? (openSerialCommand = new RelayCommand(execute: ExecuteOpenSerial));

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
            get => saveLogTextCommand ?? (saveLogTextCommand = new RelayCommand(execute: ExecuteSaveLogText));

            set => saveLogTextCommand = value;
        }

        /// <summary>
        /// Gets or sets the save once command.
        /// </summary>
        public RelayCommand SaveOnceCommand {
            get => coolNSaveCommand ?? (coolNSaveCommand = new RelayCommand(execute: ExecuteSaveOnceCommand));

            set => coolNSaveCommand = value;
        }

        /// <summary>
        /// Gets or sets the selected on baud combox command.
        /// </summary>
        public RelayCommand SelectedOnBaudComboxCommand {
            get => selectOnBaudComboxCommand ?? (selectOnBaudComboxCommand = new RelayCommand(execute: ExecuteSelected_OnBaudCombox));

            set => selectOnBaudComboxCommand = value;
        }

        /// <summary>
        /// Gets or sets the select on com combox command.
        /// </summary>
        public RelayCommand SelectOnComComboxCommand {
            get => selectOnComComboxCommand ?? (selectOnComComboxCommand = new RelayCommand(execute: ExecuteSelected_OnComCombox));

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
        /// Gets or sets the rcv data list.
        /// </summary>
        List<Queue<string>> RcvDataList { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// TODO The execute change file name.
        /// </summary>
        private void ExecuteChangeFileName()
        {
        }

        /// <summary>
        /// TODO The execute change path.
        /// </summary>
        private void ExecuteChangePath()
        {
        }

        /// <summary>
        /// TODO The execute clear log text.
        /// </summary>
        private void ExecuteClearLogText()
        {
        }

        /// <summary>
        /// The execute collect data.
        /// </summary>
        private void ExecuteCollectData()
        {
            if (!IsCollecting)
            {
                IsCollecting = true;
                ProcUnit.StartRcvData();
                ProcUnit.AutoSendAngle(ctTokenSource.Token, 2,500);
            }
            else
            {
                IsCollecting = false;
                ProcUnit.StopRcvData();
                ctTokenSource.Cancel();
            }
        }

        /// <summary>
        /// The execute open serial.
        /// </summary>
        private void ExecuteOpenSerial()
        {
            if (!IsOpen)
            {
                ProcUnit.InitPort(com: ComCollectionItem, rate: BaudCollectionItem);
                IsOpen = ProcUnit.OpenPort();
            }
            else
            {
                IsOpen = false;
                ProcUnit.ClosePort();
            }
        }

        /// <summary>
        /// TODO The execute save log text.
        /// </summary>
        private void ExecuteSaveLogText()
        {
        }

        /// <summary>
        /// TODO The execute save once command.
        /// </summary>
        private void ExecuteSaveOnceCommand()
        {
            using (FileStream fs = new FileStream(FilePath + FileName, FileMode.OpenOrCreate))
            {
            }
        }

        /// <summary>
        /// TODO The execute selected on baud combox.
        /// </summary>
        private void ExecuteSelected_OnBaudCombox()
        {
        }

        /// <summary>
        /// TODO The execute selected on com combox.
        /// </summary>
        private void ExecuteSelected_OnComCombox()
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
        private void GetDataReached(object sender, SensorEventArgs e)
        {
            if (e.Num == 0)
            {
                return;
            }

            var num = e.Num;
            string head = e.Angles.Head.ToString();
            string pitch = e.Angles.Pitch.ToString();
            string roll = e.Angles.Roll.ToString();

            SensorPanelViewModels[index: num].TextHead = head;
            SensorPanelViewModels[index: num].TextPitch = pitch;
            SensorPanelViewModels[index: num].TextRoll = roll;

            rcvStrBuilder.Append("时间: " + DateTime.Now + " ");

            rcvStrBuilder.Append("Head: " + head + " ");
            rcvStrBuilder.Append("Pitch: " + pitch + " ");
            rcvStrBuilder.Append("Roll: " + roll + " ");

            RcvDataList[num].Enqueue(rcvStrBuilder.ToString());

            rcvStrBuilder.Clear();
        }

        /// <summary>
        /// The init data.
        /// </summary>
        private void Init()
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

            ctTokenSource = new CancellationTokenSource();

            RcvDataList = new List<Queue<string>>(8);
            for (int i = 0; i < 8; i++)
            {
                RcvDataList.Add(new Queue<string>());
            }
        }
        #endregion
    }

    // #endregion
}