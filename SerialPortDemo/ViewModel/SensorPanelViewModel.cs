// 2019062615:16

namespace SerialPortDemo.ViewModel {
    using System.Collections.Generic;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// The sensor panel view model.
    /// </summary>
    public class SensorPanelViewModel : ViewModelBase
    {
        /// <summary>
        ///  the text of head.
        /// </summary>
        string textHead;

        /// <summary>
        ///  the text of pitch.
        /// </summary>
        string textPitch;

        /// <summary>
        /// The text of roll.
        /// </summary>
        string textRoll;

        /// <summary>
        ///  The lab num.
        /// </summary>
        string labNum;

        /// <summary>
        ///  The checkbox is checked.
        /// </summary>
        private bool isChecked;

        /// <summary>
        /// The width.
        /// </summary>
        private int width;

        /// <summary>
        /// The height.
        /// </summary>
        private int height;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorPanelViewModel"/> class.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="num">
        /// The address num.
        /// </param>
        /// <param name="head">
        /// The head value.
        /// </param>
        /// <param name="pitch">
        /// The pitch value.
        /// </param>
        /// <param name="roll">
        /// The roll value.
        /// </param>
        public SensorPanelViewModel(int width, int height, string num, string head, string pitch, string roll)
        {
            Width = width;
            Height = height;
            LabNum = num;
            TextHead = head;
            TextRoll = roll;
            TextPitch = pitch;
        }

        /// <summary>
        /// Gets or sets the text head.
        /// </summary>
        public string TextHead {
            get => textHead;
            set {
                textHead = value;
                RaisePropertyChanged(() => TextHead);
            }
        }

        /// <summary>
        /// Gets or sets the text pitch.
        /// </summary>
        public string TextPitch {
            get => textPitch;
            set {
                textPitch = value;
                RaisePropertyChanged(() => TextPitch);
            }
        }

        /// <summary>
        /// Gets or sets the text roll.
        /// </summary>
        public string TextRoll {
            get => textRoll;
            set {
                textRoll = value;
                RaisePropertyChanged(() => TextRoll);
            }
        }

        /// <summary>
        /// Gets or sets the lab num.
        /// </summary>
        public string LabNum {
            get => labNum;
            set {
                labNum = value;
                RaisePropertyChanged(() => LabNum);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is checked.
        /// </summary>
        public bool IsChecked {
            get => isChecked;
            set {
                isChecked = value;
                RaisePropertyChanged(IsChecked.ToString());

            }
        }

        /// <summary>
        /// The width.
        /// </summary>
        public int Width {
            get => width;
            set {
                width = value;
                RaisePropertyChanged(Width.ToString());
            }
        }

        /// <summary>
        /// The height.
        /// </summary>
        public int Height {
            get => height;
            set {
                height = value;
                RaisePropertyChanged(Height.ToString());

            }
        }
    }
}
