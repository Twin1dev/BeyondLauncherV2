using BeyondLauncherV2.Properties;
using System;
using System.Collections.Generic;
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

            }


        }


    }
}
