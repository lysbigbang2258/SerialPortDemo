namespace SerialPortDemo.View
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// MasterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MasterWindow : Window
    {
        public MasterWindow()
        {
            InitializeComponent();
            SensorPanels = new List<SensorPanelView>();
        }

        public List<SensorPanelView> SensorPanels {
            get;
            set;
        }

    }
}
