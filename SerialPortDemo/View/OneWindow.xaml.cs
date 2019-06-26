namespace SerialPortDemo {
    using System;
    using System.Windows;
    using System.Windows.Data;

    using GalaSoft.MvvmLight.Messaging;

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
            Messenger.Default.Register<String>(this, "ContentChanged", StartContentChanged);
            
            // 卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);
        }


        void StartContentChanged(string msg) {
            btnStart.Content = msg;
        }

    }
}
