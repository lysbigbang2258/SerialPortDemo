// 2019061911:01

namespace SerialPortDemo.Model {
    using System;

    public class SensorEventArgs:EventArgs{

        public SensorEventArgs(Angles angles,int num) {
            Angles = angles;
            Num = num;
        }

        public Angles Angles {
            get;
            set;
        }

        public int Num {
            get;
            set;
        }
    }
}
