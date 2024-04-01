using BeyondLauncherV2.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeyondLauncherV2.Utilities
{
    class Updater
    {
        public static bool NeedsUpdate()
        {
            using (WebClient wc = new())
            {
                string v = wc.DownloadString("http://backend.beyondfn.xyz:3551/lversion2111222");

                if (v != null)
                {
                    if (v != Resources.Version)
                    {
                     
                        return true;

                    }
                }
            }

            return false;
        }


        public static void Update()
        {
            if (!File.Exists(SimpleUtils.GetCurrentExeDirectory() + "\\BeyondUpdater.exe"))
            {
                SimpleUtils.DownloadFile("http://backend.beyondfn.xyz:3551/cdn/BeyondUpdater.exe", SimpleUtils.GetCurrentExeDirectory() + "\\BeyondUpdater.exe");

                while (!File.Exists(SimpleUtils.GetCurrentExeDirectory() + "\\BeyondUpdater.exe"))
                {
                    Task.Delay(200);
                }
            }
            Process.Start(SimpleUtils.GetCurrentExeDirectory() + "\\BeyondUpdater.exe", "\"" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName) + ".exe" + "\"");
            Environment.Exit(0);
        }


    }
}
