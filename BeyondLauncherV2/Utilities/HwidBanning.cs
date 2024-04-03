using BeyondLauncherV2.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

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

        public static string GetUUID()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");

            foreach (ManagementObject obj in searcher.Get())
            {
                string uuid = obj["UUID"].ToString();

                return uuid;
            }

            return "";
        }

        public static bool PushHWID()
        {
            string UUID = GetUUID();
           
            if (UUID == "")
                return false;

            using (WebClient wc = new())
            {
                wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                wc.Headers.Add("User-Agent", "");

                string res = wc.DownloadString("http://backend.beyondfn.xyz:8990/backend/" + Settings.Default.Email + "/pushHwidYYYY/" + UUID);

                if (res == "notfound")
                {
                    MessageBox.Show("Something has occured within our API. please try again.");
                    Environment.Exit(0);
                }

                return true;
            }
        }

        public static bool CheckForProxy()
        {
            using (WebClient sigma = new())
            {
                string url = "https://httpbin.org/ip";
                string response = sigma.DownloadString(url);
                string publicIP = response.Split('"')[3];
                MessageBox.Show(publicIP);

                string url2 = $"http://ip-api.com/json/{publicIP}?fields=proxy";

                string json = sigma.DownloadString(url2);

                bool proxyValue = bool.Parse(json.Split(':')[1].TrimEnd('}').ToLower());

                if (proxyValue)
                {
                    MessageBox.Show("VPNs are not aloud while playing Beyond. If this is a mistake, Please report it to the support server");
                    Environment.Exit(0);
                }
            }

            return false;
        }

        public static bool CheckForBan()
        {
            TryStopHttpDebugger();

            using (WebClient wc = new())
            {
                wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                string UUID = GetUUID();

                if (UUID == "")
                    return true;

                string res = wc.DownloadString($"http://backend.beyondfn.xyz:8990/backend/hwid/{UUID}/isBannedSmokearr");

                //  MessageBox.Show(res);

                if (res == "notfound")
                {
                    if (!PushHWID())
                    {
                        MessageBox.Show("Something on your Computer seems to be conflicting with the usage of beyond.. Please make sure you arent using a game spoofer.\nIf you are having this issue more than once, Please report it in the support server");
                        Environment.Exit(0);
                        return true;
                    }
                }
                else if (res == "false")
                    return false;
                else if (res == "true")
                    return true;
            }

            return true;
        }
    }
}
