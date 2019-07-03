// 2019062014:31

namespace SerialPortDemo.ViewModel {
    using System;
    using System.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SerialPortDemo.Model;

    /// <summary>
    ///     The one window model.
    /// </summary>
    public class OneWindowModel : ViewModelBase {
        /// <summary>
        ///     The sensor data.
        /// </summary>
        SensorDataModel sensordata;

        RelayCommand startclickCommand;

        RelayCommand getParmclickCommand;

        RelayCommand getAutoParmclickCommand;

        bool isOpen;

        public OneWindowModel() {
            isOpen = false;
            SensorData = new SensorDataModel();
        }

        /// <summary>
        ///     Gets or sets the sensor data.
        /// </summary>
        public SensorDataModel SensorData {
            get => sensordata;
            set {
                sensordata = value;
                RaisePropertyChanged(() => SensorData);
            }
        }

        public DataProcUnit ProcUnit {
            get;
            set;
        }

        #region 命令

        public RelayCommand BtnStartClickCommand {
            get => startclickCommand ?? (startclickCommand = new RelayCommand(execute: ExcuteStartClickCommand));

            set => startclickCommand = value;
        }

        public RelayCommand BtnGetParmclickCommand {
            get => getParmclickCommand ?? (getParmclickCommand = new RelayCommand(execute: ExcuteGetParmClickCommand));
            
            set => getParmclickCommand = value;
        }

        public RelayCommand BtnGetAutoParmclickCommand {
            get => getAutoParmclickCommand ?? (getParmclickCommand = new RelayCommand(execute: ExcuteGetAutoParmClickCommand));
            
            set => getAutoParmclickCommand = value;
        }

        void ExcuteGetAutoParmClickCommand()
        {
            CancellationToken ctToken = new CancellationToken();
            ProcUnit.AutoSendAngle(ctToken,2);
        }

        void ExcuteGetParmClickCommand() {
            ProcUnit.SendAngleCommand(2);
        }

        void ExcuteStartClickCommand() {
            if (!isOpen) {
                isOpen = true;
                ProcUnit = new DataProcUnit();
                ProcUnit.InitPort();
                ProcUnit.OpenPort();
                ProcUnit.StartRcvData();
                ProcUnit.SendEventHandler += AnglesGetReached;
                Messenger.Default.Send("关闭串口", "ContentChanged"); // 注意：token参数一致   
            }
            else {
                ProcUnit.ClosePort();
                Messenger.Default.Send("打开串口", "ContentChanged"); // 注意：token参数一致
            }
        }

        void AnglesGetReached(object sender, SensorEventArgs e) {
            if (e.Num == 2) {
                try {
                    SensorData.Head = e.Angles.Head.ToString();
                    SensorData.Roll = e.Angles.Roll.ToString();
                    SensorData.Pitch = e.Angles.Pitch.ToString();
                }
                catch(Exception exception) {
                    Console.WriteLine(value: exception);
                    throw;
                }
            }
        }

        #endregion
    }
}
