using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Utilities
{
    internal class HwidBanning
    {
        public static void TryStopHttpDebugger()
        {
            var Procs = Process.GetProcesses().Where(p => IsMatch(p, "HTTP Debugger")).ToArray();

            if (Procs.Length > 0 )
            {
                Environment.Exit(0);
            }
        }

        static bool IsMatch(Process process, string targetProductName)
        {
            try
            {
                // Check if the process has a valid module
                if (process.MainModule != null)
                {
                    // Get the product name of the process
                    string productName = FileVersionInfo.GetVersionInfo(process.MainModule.FileName).ProductName;

                    // Compare product name
                    return productName == targetProductName;
                }
            }
            catch (Exception)
            {
                // Ignore exceptions that may occur due to insufficient permissions or other reasons
            }

            return false;
        }

        public static bool CheckForBan()
        {
            TryStopHttpDebugger();
            using (WebClient wc = new())
            {
                wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                string res = wc.DownloadString("http://backend.beyondfn.xyz:8990/backend/hwid/fafafafafaf/isBannedSmokearr");

                MessageBox.Show(res);
            }

            return true;
        }
    }
}
