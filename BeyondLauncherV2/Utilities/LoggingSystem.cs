using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Utilities
{


    internal class LoggingSystem
    {
        public static readonly string BeyondFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Beyond";
        private static string LogFilePath = "";

        public static void WriteToLog(string message)
        {
            string TimeStr = DateTime.Now.ToString("HH:mm:ss");

            try
            {
                using (StreamWriter writer = File.AppendText(LogFilePath))
                {
                    writer.WriteLine($"[{TimeStr}] [BeyondLog] {message}");
                }
            }
            catch { }
        }

        public static void OpenLog()
        {
            string TimeStr = DateTime.Now.ToString("MM.dd HH.mm.ss");

            if (!Directory.Exists(BeyondFolder))
                Directory.CreateDirectory(BeyondFolder);

            if (!File.Exists(BeyondFolder + $"\\BeyondLog-{TimeStr}.txt"))
            {
                var NewLog = File.Create(BeyondFolder + $"\\BeyondLog-{TimeStr}.txt");
                NewLog.Close();

                LogFilePath = BeyondFolder + $"\\BeyondLog-{TimeStr}.txt";

                try
                {
                    using (StreamWriter writer = File.AppendText(LogFilePath))
                    {
                        writer.WriteLine("[BeyondLog] Starting");
                    }
                } catch { }
            }
        }
    }
}
