using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeyondLauncherV2.Utilities
{
    internal class SimpleUtils
    {
        public static void Restart()
        {
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();
        }

        public static void OpenLink(string link)
        {
            Process p = new Process();
            p.StartInfo.FileName = link;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }
    }
}
