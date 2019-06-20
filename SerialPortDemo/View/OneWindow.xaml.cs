namespace SerialPortDemo {
    using System;
    using System.Windows;

    using SerialPortDemo.Model;

    /// <summary>
    ///     OneWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OneWindow : Window {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneWindow"/> class.
        /// </summary>
        public OneWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the .
        /// </summary>
        DataProc Proc {
            get;
            set;
        }

        /// <summary>
        ///     The btn start_ on click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void BtnStart_OnClick(object sender, RoutedEventArgs e) {
            if ((string)btnStart.Content == "打开串口") {
                Proc = new DataProc();
                Proc.InitPort();
                Proc.OpenPort();
                Proc.SendEventHandler += AnglesGetReached;
                Proc.StartRcvData();

                tbMsg.Text += "打开串口\n";
                btnStart.Content = "关闭串口";
            }
            else {
                Proc.ClosePort();
                btnStart.Content = "打开串口";
                tbMsg.Text += "关闭串口\n";
            }
        }

        void AnglesGetReached(object sender, SensorEventArgs e) {
            if (e.Num == 2) {
                tb_secondHead.Dispatcher.InvokeAsync(
                                                    () => {
                                                        
                                                        tb_secondHead.Text = e.Angles.Head.ToString();
                                                    });
                tb_secondPitch.Dispatcher.InvokeAsync(
                                                    () => {
                                                        tb_secondPitch.Text = e.Angles.Pitch.ToString();
                                                    });
                tb_secondPitch.Dispatcher.InvokeAsync(
                                                    () => {
                                                        tb_secondRoll.Text = e.Angles.Roll.ToString();
                                                    });
            }
            tbMsg.Dispatcher.InvokeAsync(
                                         () => {
                                             tbMsg.Text += "获取数据" + DateTime.Now + "\n";
                                         });
        }

        void BtnGetParm_OnClick(object sender, RoutedEventArgs e) {
            Proc.SendAngleCommand(2);
        }

        void BtnAutoGetParm_OnClick(object sender, RoutedEventArgs e) {
            Proc.AutoSendAngle(2);
        }
    }
}
