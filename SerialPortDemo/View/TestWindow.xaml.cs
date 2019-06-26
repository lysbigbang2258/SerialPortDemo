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

    /// <summary>
    ///     TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window {

        public static readonly DependencyProperty AngleContentProperty = DependencyProperty.Register(
                                                                                                    "AngleContent",
                                                                                                    typeof(Angles),
                                                                                                    typeof(SensorPanelControl),
                                                                                                    new UIPropertyMetadata(new Angles(1, 1, 1)));


        public TestWindow() {
            InitializeComponent();
            DataContext = this;
        }

        public List<SensorPanelControl> SensorPanels {
            get;
            set;
        }

        public Angles AngleContent {
            get => (Angles)GetValue(AngleContentProperty);
            set => SetValue(AngleContentProperty, value);
        }

        void ButtonBase_OnClick(object sender, RoutedEventArgs e) {

        }



        void NewMethod() {
            int maxcount = 16;
            int invokecount = 0;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            Task.Run(
                     () => {
                         Timer timer = new Timer(
                                 s => {
                                     AutoResetEvent autoReset = (AutoResetEvent)s;

                                     invokecount++;

                                     for(int i = 0; i < 3; i++) {
                                         Dispatcher.Invoke(
                                                           () => {
                                                               SensorPanels[i].PitchContent = invokecount.ToString();
                                                               Thread.SpinWait(100);
                                                               Console.WriteLine("序号" + i + "：次数" + invokecount);
                                                           });
                                     }

                                     if (invokecount == maxcount) {
                                         autoReset.Set();
                                     }
                                 },
                                 autoEvent,
                                 0,
                                 1000);
                         autoEvent.WaitOne();
                         timer.Dispose();
                     });
        }
    }

    public class StrAngles {

        public StrAngles(string head, string pitch, string roll) {
            Head = head;
            Pitch = pitch;
            Roll = roll;
        }
        /// <summary>
        ///     Gets the head.
        /// </summary>
        public string Head {
            get;
            set;
        }

        /// <summary>
        ///     Gets the pitch.
        /// </summary>
        public string Pitch {
            get;
            set;
        }

        /// <summary>
        ///     Gets the roll.
        /// </summary>
        public string Roll {
            get;
            set;
        }
    }
}
