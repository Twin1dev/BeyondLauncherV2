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

        public static async Task<string> GetUUIDSync()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");

            foreach (ManagementObject obj in searcher.Get())
            {
                string uuid = obj["UUID"].ToString();

                return uuid;
            }

            return "";
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

        public static async Task<bool> PushHWID()
        {
            string UUID = await GetUUIDSync();
           
            if (UUID == "")
                return false;

            using (WebClient wc = new())
            {
                wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                wc.Headers.Add("User-Agent", "");

                string res = wc.DownloadString("http://backend.beyondfn.xyz:8990/backend/" + Settings.Default.Email + "/pushHwidYYYY/" + UUID);

                while (wc.IsBusy)
                {
                    Thread.Sleep(500);
                }

               

                // todo better way of this
                if (res == "already")
                {
                    return true;
                }

                int Result = await CheckForBan();

                if (Result == 0x1)
                {
                    MessageBox.Show("You are Currently Banned from Beyond. If this is a mistake, Please go to the support server!");
                    Environment.Exit(0);
                    return true;
                } else if (Result == 0x190)
                {
                    MessageBox.Show("An error occured, Please make a ticket in the support server.\n\nError Code: 0x190");
                    Environment.Exit(0);
                    return true;
                } else if (Result == 0x180) {
                    MessageBox.Show("An error occured, Please make a ticket in the support server.\n\nError Code: 0x180");
                    Environment.Exit(0);
                    return true;
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

        public static async Task<int> CheckForBan()
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                    string UUID = await GetUUIDSync();

                    if (UUID == "")
                    {
                        LoggingSystem.WriteToLog("Checking for Ban returned a code: 0x180");
                        return 0x180;
                    }

                    string res = wc.DownloadString("http://backend.beyondfn.xyz:8990/backend/hwid/" + UUID + "/isBannedSmokearr");

                    while (wc.IsBusy)
                    {
                        Thread.Sleep(500);
                    }

                    if (res == "notfound")
                    {
                        LoggingSystem.WriteToLog("Checking for Ban returned a code: 0x190");
                        return 0x190;
                    }
                    else if (res == "false")
                    {
                        // Handle false case
                        return 0x0;
                    }
                    else if (res == "true")
                    {
                        return 0x1;
                    }
                    else
                    {
                        // Handle unexpected response
                        LoggingSystem.WriteToLog("Unexpected response: " + res);
                        return 0x1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                LoggingSystem.WriteToLog("An error occurred: " + ex.Message);
                return 0x1;
            }
        }


        /*        public static async Task<bool> CheckForBan()
                {
                    //TryStopHttpDebugger();
                    bool ret = false;
                    using (WebClient wc = new())
                    {
                        wc.Headers.Add("authkey", SimpleUtils.TimeStampEncryption());
                        string UUID = await GetUUIDSync();

                        if (UUID == "")
                        {
                            LoggingSystem.WriteToLog("Checking for Ban returned a code: 0x180");
                            return true; 
                        }

                        string res = wc.DownloadString(new Uri("http://backend.beyondfn.xyz:8990/backend/hwid/" + UUID + "/isBannedSmokearr"));



                        wc.DownloadStringCompleted += (s, e) =>
                        {
                            if (res == "notfound")
                            {
                                LoggingSystem.WriteToLog("Checking for Ban returned a code: 0x190");
                                ret = true;
                                *//*  if (!PushHWID())
                                  {
                                      MessageBox.Show("Something on your Computer seems to be conflicting with the usage of beyond.. Please make sure you arent using a game spoofer.\nIf you are having this issue more than once, Please report it in the support server");
                                      Environment.Exit(0);
                                      return true;
                                  }*//*
                            }
                            else if (res == "false")
                            {
                                // i dont think we need to use the absoulute value?
                                try
                                {
                                    string authkey = Encoding.UTF8.GetString(Convert.FromBase64String(wc.ResponseHeaders.Get("authkey")));
                                    int authkeyseconds = Int32.Parse(authkey.Split('?').Last());

                                    int currentDateTimeSeconds = Int32.Parse(DateTime.UtcNow.ToString("ss"));

                                    int TimeDiff = authkeyseconds - currentDateTimeSeconds;

                                    // MessageBox.Show(TimeDiff.ToString());
                                    if (TimeDiff > 8)
                                    {
                                        LoggingSystem.WriteToLog("Checking for Ban returned a code: 0x100");
                                        ret = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occured. Code: 0x130");
                                    Environment.Exit(0);
                                }

                                ret = false;
                            }
                            else if (res == "true")
                                ret = true;
                        };

                        //  MessageBox.Show(res);
                    }

                    return ret;
                }*/
    }
}
