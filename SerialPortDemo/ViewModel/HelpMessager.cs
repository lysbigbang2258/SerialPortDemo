// 201908129:50

namespace SerialPortDemo.ViewModel
{
    using System;
    using System.IO;
    using System.Text;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// The help messenger.
    /// </summary>
    public class HelpMessenger : ObservableObject
    {
        /// <summary>
        /// The message builder.
        /// </summary>
        private readonly StringBuilder messengeBuilder;

        /// <summary>
        /// The message.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpMessenger"/> class. 
        /// </summary>
       public HelpMessenger()
        {
            messengeBuilder = new StringBuilder();
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get => message;
            set {
                message = value;
                RaisePropertyChanged(() => Message);
            } }

        /// <summary>
        /// The send message.
        /// </summary>
        /// <param name="messagedata">
        /// The message.
        /// </param>
        public void AddMessage(string messagedata)
        {
            messengeBuilder.Append(DateTime.Now);
            messengeBuilder.AppendLine(messagedata);
            Message = messengeBuilder.ToString();
        }

        /// <summary>
        /// The clear messger.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ClearMessger()
        {
            try
            {
            }
            finally
            {
                messengeBuilder.Clear();
                Message = string.Empty;
            }
            return true;
        }

        /// <summary>
        /// TDO The save messenger.
        /// </summary>
        public void SaveMessenger()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo info = new DirectoryInfo(basePath);
            info.CreateSubdirectory("LogData");
            string pathString = Path.Combine(basePath, "LogData");
            string strTime = DateTime.Now.ToString("yyyy_mm_dd_hh_mm_ss");
            string fileName = "Log" + strTime + ".txt";
            
            string path = Path.Combine(pathString, fileName);

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GB2312")))
                    {
                        sw.Write(Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}