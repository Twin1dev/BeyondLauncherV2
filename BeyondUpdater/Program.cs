using System.Diagnostics;
using System.Net;

namespace BeyondUpdater
{
    internal class Logger
    {
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(args[0]))
            {
                try { File.Delete(args[0]); } catch {
                    Logger.Error("There was an Error While Updating! Err: Could not Delete Old Launcher.");
                    Console.WriteLine("\nTry Running the Launcher as Admin!");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }

                using (WebClient wc = new())
                {
                    wc.DownloadFile("http://backend.beyondfn.xyz:3551/launcherdl", args[0]);
                    wc.DownloadProgressChanged += ProgressChanged;

                    while (wc.IsBusy)
                    {
                        Thread.Sleep(100);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Updated!");
                Process.Start(args[0]);
                Thread.Sleep(5000);

            } else
            {
                Logger.Error("There was an Error While Updating! Err: Could not Find File");
                Thread.Sleep(5000);
            }
        }


        static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive <= 0)
            { return; }
            int percentage = (int)((double)e.BytesReceived / e.TotalBytesToReceive * 100);

            Console.Write("\rProgress: [{0}{1}] {2}%   ",
                new string('#', percentage / 5), new string(' ', 20 - percentage / 5), percentage);

            if (percentage == 100)
            {
                Console.WriteLine();
            }
        }
    }

}
