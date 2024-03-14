using System.Diagnostics;
using System.Net;
using System.Windows;

namespace BeyondLauncherV2.Utilities
{
    internal class SimpleUtils
    {
        public static void DownloadFile(string URL, string path)
        {
            new WebClient().DownloadFile(URL, path);
        }

        public static void SafeKillProcess(string processName)
        {
            try
            {
                Process[] processesByName = Process.GetProcessesByName(processName);
                for (int i = 0; i < processesByName.Length; i++)
                {
                    processesByName[i].Kill();
                }
            }
            catch
            {
            }
        }

        private readonly static string[] skinlinks = ["https://fortnite-api.com/images/cosmetics/br/cid_a_406_athena_commando_m_rebirthfresh/icon.png", "https://fortnite-api.com/images/cosmetics/br/CID_701_Athena_Commando_M_BananaAgent/icon.png", "https://fortnite-api.com/images/cosmetics/br/CID_703_Athena_Commando_M_Cyclone/icon.png", "https://fortnite-api.com/images/cosmetics/br/CID_691_Athena_Commando_F_TNTina/icon.png", "https://fortnite-api.com/images/cosmetics/br/CID_694_Athena_Commando_M_CatBurglar/icon.png"];

        public static string GetRandomSkinLink()
        {
            Random rnd = new();
            return skinlinks[rnd.Next(0, skinlinks.Length)];
        }

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
