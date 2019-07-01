namespace SerialPortDemo.View {
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using SerialPortDemo.ViewModel;

    /// <summary>
    ///     SensorPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class SensorPanelView : UserControl {
        // public static readonly DependencyProperty HeadContentProperty = DependencyProperty.Register(
        //                                                                                             "HeadContent",
        //                                                                                             typeof(string),
        //                                                                                             typeof(SensorPanelView),
        //                                                                                             new UIPropertyMetadata("0"));
        //
        // public static readonly DependencyProperty PitchContentProperty = DependencyProperty.Register(
        //                                                                                              "PitchContent",
        //                                                                                              typeof(string),
        //                                                                                              typeof(SensorPanelView),
        //                                                                                              new UIPropertyMetadata("0"));
        //
        // public static readonly DependencyProperty RollContentProperty = DependencyProperty.Register(
        //                                                                                             "RollContent",
        //                                                                                             typeof(string),
        //                                                                                             typeof(SensorPanelView),
        //                                                                                             new UIPropertyMetadata("0"));
        //
        // public static readonly DependencyProperty LabNumProperty = DependencyProperty.Register(
        //                                                                                             "LabNum",
        //                                                                                             typeof(string),
        //                                                                                             typeof(SensorPanelView),
        //                                                                                             new UIPropertyMetadata("0"));

        public SensorPanelView(SensorPanelViewModel viewModel) {
            InitializeComponent();

            DataContext = viewModel;
        }

        int id;


        public void SetContent(int name) {
            labelSensorId.Content = name.ToString();
        }

        // public string HeadContent {
        //     get => (string)GetValue(HeadContentProperty);
        //     set => SetValue(HeadContentProperty, value);
        // }
        //
        // public string PitchContent {
        // get => (string)GetValue(PitchContentProperty);
        // set => SetValue(PitchContentProperty, value);
        // }
        //
        // public string RollContent {
        // get => (string)GetValue(RollContentProperty);
        // set => SetValue(RollContentProperty, value);
        // }
        //
        // public string LabNum {
        //     get => (string)GetValue(LabNumProperty);
        //     set => SetValue(LabNumProperty, value);
        // }
    }
}
