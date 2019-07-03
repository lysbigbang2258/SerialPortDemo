// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   MainWindow.xaml 的交互逻辑
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SerialPortDemo.View {
    using System;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    using SerialPortDemo.Model;

    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            DataProc = new DataProcUnit();
            InitPortProperty();
        }

        private DataProcUnit DataProc { get; set; }

        /// <summary>
        ///     The init port property.
        /// </summary>
        void InitPortProperty() {
            if (!DataProc.InitPort("COM3", 19200)) {
                MessageBox.Show("端口未连接");
            }

            var baudRateCollection = new ObservableCollection<int> { 2400, 4800, 9600, 19200, 38400, 38400, 57600, 115200 };

            combPort.ItemsSource = DataProc.GetPortNames();
            combPort.SelectedIndex = 0;
            
            combBaudRate.ItemsSource = baudRateCollection;
            combBaudRate.SelectedIndex = 2;
        }

        /// <summary>
        ///     The port_ rcv byte reached.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void Port_RcvByteReached(object sender, byte[] e) {
            StringBuilder sb = new StringBuilder(e.Length * 3);
            foreach(byte b in e) {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            }

            string str = sb.ToString().ToUpper();
            tbReceive.Dispatcher.InvokeAsync(
                                             () => {
                                                 tbReceive.Text += str;
                                             });
        }

        /// <summary>
        ///     The bt open_ on click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void BtOpenOrClose_OnClick(object sender, RoutedEventArgs e) {
            if (btOpenOrClose.Content.ToString() == "停止") {
                if (DataProc.IsPortOpen) {
                    DataProc.ClosePort();
                    btOpenOrClose.Content = "连接";
                    labelConnect.Content = "关闭";
                }
                else {
                    MessageBox.Show("串口已关闭");
                }
            }
            else if (btOpenOrClose.Content.ToString() == "连接") {
                if (!DataProc.IsPortOpen) {
                    DataProc.OpenPort();
                    btOpenOrClose.Content = "停止";
                    labelConnect.Content = "已开启";
                }
                else {
                    MessageBox.Show("串口已打开");
                }
            }
            else {
                MessageBox.Show("错误");
            }
        }

        /// <summary>
        ///     The btn send_ on click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void BtnSend_OnClick(object sender, RoutedEventArgs e) {
            // DataProc.SendData(,tbSend.Text);
        }

        /// <summary>
        ///     The btn clear send_ on click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void BtnClearSend_OnClick(object sender, RoutedEventArgs e) {
            tbSend.Text = string.Empty;
        }

        /// <summary>
        ///     The btn clear rcv_ on click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        void BtnClearRcv_OnClick(object sender, RoutedEventArgs e) {
            tbReceive.Text = string.Empty;
        }



        void CombPort_OnSelectionChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
