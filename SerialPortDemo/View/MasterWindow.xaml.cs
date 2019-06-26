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
            SensorPanels = new List<SensorPanelControl>();
            InitGridMsg();
        }

        public List<SensorPanelControl> SensorPanels {
            get;
            set;
        }
        void InitGridMsg() {
            int id = 0;
            for(int i = 0; i < 5; i++) {
                var panel = (DockPanel)gridMsg.Children[i];
                panel.Margin = new Thickness(5);
            }
        }


    }
}
