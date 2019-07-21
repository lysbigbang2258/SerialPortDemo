// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SensorPanelView.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   SensorPanelView.xaml 的交互逻辑
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SerialPortDemo.View {
    using SerialPortDemo.ViewModel;

    /// <summary>
    ///     SensorPanelView.xaml 的交互逻辑
    /// </summary>
    public partial class SensorPanelView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SensorPanelView"/> class.
        /// </summary>
        /// <param name="viewModel">
        /// The SensorPanel view model.
        /// </param>
        public SensorPanelView(SensorPanelViewModel viewModel) {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
