﻿using System.Diagnostics;
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
            try
            {
                using (WebClient wc = new())
                {
                    wc.DownloadFile(URL, path);

                    while (wc.IsBusy)
                    {
                        Thread.Sleep(100);
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("An error occured while downloading a file, please consider running as admin");
                Environment.Exit(0);
            }
           
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
            string DateTimeStr = DateTime.UtcNow.ToString("dd?MM?ss");
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTimeStr));
        }

        public static void SafeKillProcess(string processName)
        {
            if (processName == "")
                return;

            try
            {
                Process[] processesByName = Process.GetProcessesByName(processName);
                for (int i = 0; i < processesByName.Length; i++)
                {
                    processesByName[i].Kill();
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("An Error occured, Error data has been saved to the Logs located in Documents");
               //LoggingSystem.WriteToLog(ex.ToString());
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
            Process p = new();
            p.StartInfo.FileName = link;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        public static bool VerifyStaff()
        {
            return true;
        }

        public static string GetCurrentExeDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
