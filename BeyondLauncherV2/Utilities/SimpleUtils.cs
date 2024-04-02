using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace BeyondLauncherV2.Utilities
{
    internal class SimpleUtils
    {
        public static void DownloadFile(string URL, string path)
        {
            new WebClient().DownloadFile(URL, path);
        }

        public static string ComputeHMACSHA256(string data, string key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public static string TimeStampEncryption()
        {
            string DateTimeStr = DateTime.UtcNow.ToString("dd?MM?mm"); // UTC time format
          //  new Thread(() => { MessageBox.Show(DateTimeStr); }).Start();
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTimeStr));
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
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName) + ".exe");
            Application.Current.Shutdown();
        }

        public static void OpenLink(string link)
        {
            Process p = new Process();
            p.StartInfo.FileName = link;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        public static string GetCurrentExeDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
