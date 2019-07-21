// 2019062416:34

namespace SerialPortDemo.ViewModel
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Windows;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SerialPortDemo.Model;
    using SerialPortDemo.View;

    /// <summary>
    ///  The master window model.
    /// </summary>
    public class MasterWindowModel : ViewModelBase
    {
        #region Field

        /// <summary>
        ///     The receive Sensor data string builder
        /// </summary>
        private readonly StringBuilder rcvStrBuilder;

        /// <summary>
        ///     The baud collection binding com-box.
        /// </summary>
        private ObservableCollection<int> baudCollection;

        /// <summary>
        ///     The baud collection item is Selected.
        /// </summary>
        private int baudCollectionItem;

        /// <summary>
        ///     The command changing file name.
        /// </summary>
        private RelayCommand changeFileNameCommand;

        /// <summary>
        ///     The command of changing path.
        /// </summary>
        private RelayCommand changePathCommand;

        /// <summary>
        ///     The command of clear log text.
        /// </summary>
        private RelayCommand clearLogTextCommand;

        /// <summary>
        ///     The command of collecting data.
        /// </summary>
        private RelayCommand collectDataCommand;

        /// <summary>
        ///     The Com collection binding com-box.
        /// </summary>
        private ObservableCollection<string> comCollection;

        /// <summary>
        ///     The com collection item is Selected.
        /// </summary>
        private string comCollectionItem;

        /// <summary>
        ///     The command of saving once  data command.
        /// </summary>
        private RelayCommand coolNSaveCommand;

        /// <summary>
        ///     Now The current time data.
        /// </summary>
        private string curTime;

        /// <summary>
        ///     The saved file name.
        /// </summary>
        private string fileName;

        /// <summary>
        ///     The saved file path.
        /// </summary>
        private string filePath;

        /// <summary>
        ///     The command of opening serial .
        /// </summary>
        private RelayCommand openSerialCommand;

        /// <summary>
        ///     The proc unit object.
        /// </summary>
        private DataProcUnit procUnit;

        /// <summary>
        ///     The command of logging text.
        /// </summary>
        private RelayCommand saveLogTextCommand;

        /// <summary>
        ///     The command of selecting on baud com-box.
        /// </summary>
        private RelayCommand selectOnBaudComboxCommand;

        /// <summary>
        ///     The command of selecting com com-box.
        /// </summary>
        private RelayCommand selectOnComComboxCommand;

        /// <summary>
        ///     The log or command text msg.
        /// </summary>
        private string textMsg;

        /// <summary>
        ///     The address dictionary.
        /// </summary>
        private Dictionary<int, bool> addressDictionary;

        /// <summary>
        ///     The is single send.
        /// </summary>
        private bool isSingleSend;

        /// <summary>
        ///     The is collecting.
        /// </summary>
        private bool isCollecting;

        /// <summary>
        ///     The is start save filed.
        /// </summary>
        private bool isStartSaveFile;

        /// <summary>
        /// The save event handler.
        /// </summary>
        public event EventHandler<SaveEventArgs> SaveEventHandler;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterWindowModel" /> class.
        /// </summary>
        public MasterWindowModel()
        {
            rcvStrBuilder = new StringBuilder();
            Init();
            ProcUnit.SendEventHandler += GetDataReached;
        }

        #region Property

        /// <summary>
        /// Gets or sets a value indicating whether is port open.
        /// </summary>
        public bool IsPortOpen {
            get => ProcUnit.IsOpenPort;
        }

        /// <summary>
        ///  Gets or sets a value indicating whether is single send.
        /// </summary>
        public bool IsSingleSend {
            get => isSingleSend;
            set {
                isSingleSend = value;
                RaisePropertyChanged(() => IsSingleSend);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether is collecting.
        /// </summary>
        public bool IsCollecting {
            get => isCollecting;
            set {
                isCollecting = value;
                RaisePropertyChanged(() => IsCollecting);
            }
        }

        /// <summary>
        ///     Gets or sets the baud collection.
        /// </summary>
        public ObservableCollection<int> BaudCollection {
            get => baudCollection;

            set {
                baudCollection = value;
                RaisePropertyChanged(() => BaudCollection);
            }
        }

        /// <summary>
        ///     Gets or sets the baud collection item.
        /// </summary>
        public int BaudCollectionItem {
            get => baudCollectionItem;
            set {
                baudCollectionItem = value;
                RaisePropertyChanged(() => BaudCollectionItem);
            }
        }

        /// <summary>
        ///     Gets or sets the change file name.
        /// </summary>
        public RelayCommand ChangeFileNameCommand {
            get => changeFileNameCommand ?? (changeFileNameCommand = new RelayCommand(execute: ExecuteChangeFileName));

            set => changePathCommand = value;
        }

        /// <summary>
        ///     Gets or sets the change path.
        /// </summary>
        public RelayCommand ChangePathCommand {
            get => changePathCommand ?? (changePathCommand = new RelayCommand(execute: ExecuteChangePath));

            set => changePathCommand = value;
        }

        /// <summary>
        ///     Gets or sets the clear log text.
        /// </summary>
        public RelayCommand ClearLogTextCommand {
            get => clearLogTextCommand ?? (clearLogTextCommand = new RelayCommand(execute: ExecuteClearLogText));

            set => clearLogTextCommand = value;
        }

        /// <summary>
        ///     Gets or sets the collect data.
        /// </summary>
        public RelayCommand CollectDataCommand {
            get => collectDataCommand ?? (collectDataCommand = new RelayCommand(execute: ExecuteCollectData));
            set => collectDataCommand = value;
        }

        /// <summary>
        ///     Gets or sets the com collection.
        /// </summary>
        public ObservableCollection<string> ComCollection {
            get => comCollection;

            set {
                comCollection = value;
                RaisePropertyChanged(() => ComCollection);
            }
        }

        /// <summary>
        ///     Gets or sets the com collection item.
        /// </summary>
        public string ComCollectionItem {
            get => comCollectionItem;
            set {
                comCollectionItem = value;
                RaisePropertyChanged(() => ComCollectionItem);
            }
        }

        /// <summary>
        ///     Gets or sets the cur time.
        /// </summary>
        public string CurTime {
            get => curTime;

            set {
                curTime = value;
                RaisePropertyChanged(() => CurTime);
            }
        }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName {
            get => fileName;

            set {
                fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        /// <summary>
        ///     Gets or sets the file path.
        /// </summary>
        public string FilePath {
            get => filePath;
            set {
                filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

        /// <summary>
        ///     Gets or sets the open serial.
        /// </summary>
        public RelayCommand OpenSerialCommand {
            get => openSerialCommand ?? (openSerialCommand = new RelayCommand(execute: ExecuteOpenSerial));

            set => openSerialCommand = value;
        }

        /// <summary>
        ///     Gets or sets the proc unit.
        /// </summary>
        public DataProcUnit ProcUnit {
            get => procUnit;
            set {
                procUnit = value;
                RaisePropertyChanged(() => procUnit);
            }
        }

        /// <summary>
        ///     Gets or sets the save log text.
        /// </summary>
        public RelayCommand SaveLogTextCommand {
            get => saveLogTextCommand ?? (saveLogTextCommand = new RelayCommand(execute: ExecuteSaveLogText));

            set => saveLogTextCommand = value;
        }

        /// <summary>
        ///     Gets or sets the save once command.
        /// </summary>
        public RelayCommand SaveOnceCommand {
            get => coolNSaveCommand ?? (coolNSaveCommand = new RelayCommand(execute: ExecuteSaveOnceCommand));

            set => coolNSaveCommand = value;
        }

        /// <summary>
        ///     Gets or sets the selected on baud combox command.
        /// </summary>
        public RelayCommand SelectedOnBaudComboxCommand {
            get => selectOnBaudComboxCommand ?? (selectOnBaudComboxCommand = new RelayCommand(execute: ExecuteSelected_OnBaudCombox));

            set => selectOnBaudComboxCommand = value;
        }

        /// <summary>
        ///     Gets or sets the select on com combox command.
        /// </summary>
        public RelayCommand SelectOnComComboxCommand {
            get => selectOnComComboxCommand ?? (selectOnComComboxCommand = new RelayCommand(execute: ExecuteSelected_OnComCombox));

            set => selectOnComComboxCommand = value;
        }

        /// <summary>
        ///     Gets or sets the sensor panel view models.
        /// </summary>
        public ObservableCollection<SensorPanelViewModel> SensorPanelViewModels { get; set; }

        /// <summary>
        ///     Gets or sets the sensor panel views.
        /// </summary>
        public ObservableCollection<SensorPanelView> SensorPanelViews { get; set; }

        /// <summary>
        ///     Gets or sets the text msg.
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
        private List<ConcurrentQueue<string>> RcvDataList { get; set; }

        #endregion

        #region Method

        /// <summary>
        ///     The on save event handler.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        private void OnSaveEventHandler(SaveEventArgs args)
        {
            SaveEventHandler?.Invoke(this, e: args);
        }

        /// <summary>
        ///     The save string data.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void SaveStringData(object sender, SaveEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            
            int num = e.Num;

            string data = e.Data;

            using (StreamWriter streamWriter = new StreamWriter(FilePath + "\\" + FileName + num, true))
            {
                streamWriter.Write(value: data);
            }
        }

        /// <summary>
        ///     TODO The execute change file name.
        /// </summary>
        private void ExecuteChangeFileName()
        {
        }

        /// <summary>
        ///     TODO The execute change path.
        /// </summary>
        private void ExecuteChangePath()
        {
        }

        /// <summary>
        ///     TODO The execute clear log text.
        /// </summary>
        private void ExecuteClearLogText()
        {
        }

        /// <summary>
        ///     The execute collect data.
        /// </summary>
        private void ExecuteCollectData()
        {
            if (!IsPortOpen)
            {
                MessageBox.Show("请绑定端口！");
                return;
            }

            if (!IsCollecting)
            {
                for (int i = 0; i < SensorPanelViewModels.Count; i++)
                {
                    if (SensorPanelViewModels[index: i].IsChecked)
                    {
                        addressDictionary[key: i] = true;
                    }
                    else
                    {
                        addressDictionary[key: i] = false;
                    }
                }

                ProcUnit.StartRcvData();

                if (IsSingleSend)
                {
                    ProcUnit.CollectedData(addresses: addressDictionary);
                    IsCollecting = false;
                }
                else
                {
                    ProcUnit.StartAutoCollectData(addresses: addressDictionary);
                    IsCollecting = true;
                }
            }
            else
            {
                ProcUnit.StopCollectData();
                IsCollecting = false;
            }
        }

        /// <summary>
        ///     The execute open serial.
        /// </summary>
        private void ExecuteOpenSerial()
        {
            if (!ProcUnit.IsOpenPort)
            {
                ProcUnit.SetPortParam(portName: ComCollectionItem, baudRate: BaudCollectionItem);
                if (!ProcUnit.OpenPort())
                {
                    MessageBox.Show("端口已被占用");
                }
            }
            else
            {
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
        ///     The execute save once command.
        /// </summary>
        private void ExecuteSaveOnceCommand()
        {
            if (!isStartSaveFile)
            {
                isStartSaveFile = true;
                SaveEventHandler += SaveStringData;
            }
            else
            {
                isStartSaveFile = false;
                SaveEventHandler -= SaveStringData;
            }
        }

        /// <summary>
        ///     TODO The execute selected on baud combox.
        /// </summary>
        private void ExecuteSelected_OnBaudCombox()
        {
        }

        /// <summary>
        ///     TODO The execute selected on com combox.
        /// </summary>
        private void ExecuteSelected_OnComCombox()
        {
        }

        /// <summary>
        ///     The get data reached.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void GetDataReached(object sender, SensorEventArgs e)
        {
            try
            {
                bool tmp = Math.Abs(value: e.Angles.Head) < double.Epsilon && Math.Abs(value: e.Angles.Pitch) < double.Epsilon
                           && Math.Abs(value: e.Angles.Roll) < double.Epsilon;
                if (e.Num == 0 || tmp)
                {
                    Console.WriteLine("SensorEventArgs is null");
                    return;
                }

                var num = e.Num - 1;
                string head = e.Angles.Head.ToString();
                string pitch = e.Angles.Pitch.ToString();
                string roll = e.Angles.Roll.ToString();

                SensorPanelViewModels[index: num].TextHead = head;
                SensorPanelViewModels[index: num].TextPitch = pitch;
                SensorPanelViewModels[index: num].TextRoll = roll;

                Console.WriteLine("UI Data OK" + head + pitch + roll);

                rcvStrBuilder.Append("Time: " + DateTime.Now + " ");
                rcvStrBuilder.Append("Head: " + head + " ");
                rcvStrBuilder.Append("Pitch: " + pitch + " ");
                rcvStrBuilder.Append("Roll: " + roll + " ");
                rcvStrBuilder.Append("\n");

                OnSaveEventHandler(new SaveEventArgs(num: num, rcvStrBuilder.ToString()));

                rcvStrBuilder.Clear();
            }
            catch (Exception exception)
            {
                Console.WriteLine(value: exception);
                throw;
            }
        }

        /// <summary>
        ///     The init data.
        /// </summary>
        private void Init()
        {
            ProcUnit = new DataProcUnit();

            addressDictionary = new Dictionary<int, bool>();
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
                string str = (i + 1).ToString();
                SensorPanelViewModel viewModel = new SensorPanelViewModel(num: str, head: str, pitch: str, roll: str) { IsChecked = false };
                addressDictionary.Add(key: i, false);
                SensorPanelView view = new SensorPanelView(viewModel: viewModel);
                SensorPanelViews.Add(item: view);
                SensorPanelViewModels.Add(item: viewModel);
            }

            RcvDataList = new List<ConcurrentQueue<string>>(8);

            for (int i = 0; i < 8; i++)
            {
                RcvDataList.Add(new ConcurrentQueue<string>());
            }

            FilePath = "C:\\Record";
            FileName = "Test";
        }

        #endregion
    }

    // #endregion
}