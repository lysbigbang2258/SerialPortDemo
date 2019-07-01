namespace SerialPortDemo.View {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    using SerialPortDemo.Model;
    using SerialPortDemo.ViewModel;

    /// <summary>
    ///     TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window {
        public static readonly DependencyProperty AngleContentProperty = DependencyProperty.Register(
                                                                                                     "AngleContent",
                                                                                                     typeof(Angles),
                                                                                                     typeof(SensorPanelView),
                                                                                                     new UIPropertyMetadata(new Angles(1, 1, 1)));

        public TestWindow() {
            InitializeComponent();
            DataContext = this;
            SensorPanelViews = new ObservableCollection<SensorPanelView>();
            SensorPanelViewModels = new ObservableCollection<SensorPanelViewModel>();

            for(int i = 0; i < 8; i++) {
                string str = i.ToString();
                SensorPanelViewModel viewModel = new SensorPanelViewModel(num: str, head: str, pitch: str, roll: str);
                SensorPanelView view = new SensorPanelView(viewModel: viewModel);
                SensorPanelViews.Add(item: view);
                SensorPanelViewModels.Add(item: viewModel);
            }
        }

        public ObservableCollection<SensorPanelView> SensorPanelViews {
            get;
            set;
        }

        public ObservableCollection<SensorPanelViewModel> SensorPanelViewModels {
            get;
            set;
        }

        public Angles AngleContent {
            get => (Angles)GetValue(dp: AngleContentProperty);
            set => SetValue(dp: AngleContentProperty, value: value);
        }

        void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            int maxcount = 100;
            int invokecount = 0;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            Task.Run(
                     () => {
                         Timer timer = new Timer(
                                 s => {
                                     AutoResetEvent autoReset = (AutoResetEvent)s;
                                     invokecount++;

                                     Dispatcher.Invoke(
                                                       () => {
                                                           SensorPanelViewModels[index: 2].TextPitch = invokecount.ToString();
                                                           Thread.SpinWait(1000);
                                                           Console.WriteLine(invokecount);
                                                       });
                                    
                                     if (invokecount == maxcount) {
                                         autoReset.Set();
                                         }
                                 },
                                 autoEvent,
                                 1000,2000);
                         autoEvent.WaitOne();
                         timer.Dispose();
                     });
        }
        
    }
        
    // void NewMethod() {
            // int maxcount = 16;
            // int invokecount = 0;
            // AutoResetEvent autoEvent = new AutoResetEvent(false);
            // Task.Run(
            // () => {
            // Timer timer = new Timer(
            // s => {
            // AutoResetEvent autoReset = (AutoResetEvent)s;
            // invokecount++;
            // for(int i = 0; i < 3; i++) {
            // Dispatcher.Invoke(
            // () => {
            // SensorPanels[i].PitchContent = invokecount.ToString();
            // Thread.SpinWait(100);
            // Console.WriteLine("序号" + i + "：次数" + invokecount);
            // });
            // }
            // if (invokecount == maxcount) {
            // autoReset.Set();
            // }
            // },
            // autoEvent,
            // 0,
            // 1000);
            // autoEvent.WaitOne();
            // timer.Dispose();
            // });
            // }
        // }
    }
