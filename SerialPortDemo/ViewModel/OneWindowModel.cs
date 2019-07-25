// 2019062014:31

namespace SerialPortDemo.ViewModel
{
    using System;
    using System.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SerialPortDemo.Model;

    /// <summary>
    ///     The one window model.
    /// </summary>
    public class OneWindowModel : ViewModelBase
    {
        /// <summary>
        ///     The sensor data.
        /// </summary>
        private SensorDataModel sensordata;

        private RelayCommand startclickCommand;

        private RelayCommand getParmclickCommand;

        private RelayCommand getAutoParmclickCommand;

        private bool isOpen;

        public OneWindowModel()
        {
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

        public DataProcUnit ProcUnit { get; set; }

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

        private void ExcuteGetAutoParmClickCommand()
        {
        }

        private void ExcuteGetParmClickCommand()
        {
            ProcUnit.IsCollected = true;
            ProcUnit.SendCommand(1);
        }

        private void ExcuteStartClickCommand()
        {
            if (!isOpen)
            {
                isOpen = true;
                ProcUnit = new DataProcUnit();
                ProcUnit.SetPortParam();
                ProcUnit.OpenPort();
                ProcUnit.StartRcvData();
                ProcUnit.SendEventHandler += AnglesGetReached;
                Messenger.Default.Send("关闭串口", "ContentChanged"); // 注意：token参数一致   
            }
            else
            {
                ProcUnit.ClosePort();
                Messenger.Default.Send("打开串口", "ContentChanged"); // 注意：token参数一致
            }
        }

        /// <summary>
        /// The angles get reached.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void AnglesGetReached(object sender, SensorEventArgs e)
        {
            try
            {
                if (e.Num == 0)
                {
                    return;
                }

                SensorData.Head = e.Angles.Head.ToString();
                SensorData.Roll = e.Angles.Roll.ToString();
                SensorData.Pitch = e.Angles.Pitch.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(value: exception);
                throw;
            }
        }

        #endregion
    }
}