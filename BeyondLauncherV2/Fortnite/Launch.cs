using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using Microsoft.Win32;
using MS.WindowsAPICodePack.Internal;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

namespace BeyondLauncherV2.Fortnite
{
    internal class EAC
    {

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }

        public static string NewFileName = "";
        public static string NewFilePath = "";
        public static bool bProcessFound = false;

        // idfk what to name this
        public static void InitEAC()
        {
            Random r = new Random();
            int rInt = r.Next(8, 16);

            NewFileName = /*"Beyond";*/ GenerateRandomString(rInt);
            NewFilePath = $"\\{NewFileName}.exe";

            File.WriteAllBytes(Settings.Default.Path + NewFilePath, Resources.Beyond);

            while (!File.Exists(Settings.Default.Path + NewFilePath))
            {
                Thread.Sleep(500);
            }

            Process proc = new();
            proc.StartInfo.FileName = Settings.Default.Path + NewFilePath;
            proc.StartInfo.Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck -AUTH_TYPE=EPIC -AUTH_LOGIN=" + Settings.Default.Email + " -AUTH_PASSWORD=" + Settings.Default.Password;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();

            DateTime startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds < 45)
            {
                if (Process.GetProcessesByName(NewFileName).Length > 0)
                {
                    bProcessFound = true;

                    if (Settings.Default.CloseOnLaunch)
                    {
                        Environment.Exit(0);
                    }

                    break;
                }

                Thread.Sleep(500);
            }


            if (!bProcessFound)
            {
                foreach (var procsigma in Process.GetProcessesByName("FortniteClient-Win64-Shipping"))
                {
                    procsigma.Kill();
                }
            }

            DateTime startTime2 = DateTime.Now;
            while ((DateTime.Now - startTime2).TotalSeconds < 45)
            {

            }

            string pak;
            if (Anticheat.Scan(out pak))
            {
                foreach (var procsigma in Process.GetProcessesByName("FortniteClient-Win64-Shipping"))
                {
                    procsigma.Kill();
                }
                MessageBox.Show($"Cheating Detected! Pak Detected: {pak}");
                return;
            }
        }
    }
    public class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);


        public static bool Inject(int pid, string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            IntPtr hProcess = Win32.OpenProcess(1082, false, pid);
            if (hProcess == IntPtr.Zero)
            {
                return false;
            }

            IntPtr procAddress = Win32.GetProcAddress(Win32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (procAddress == IntPtr.Zero)
            {
                return false;
            }

            uint num = checked((uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))));
            IntPtr intPtr = Win32.VirtualAllocEx(hProcess, IntPtr.Zero, num, 12288U, 4U);
            if (intPtr == IntPtr.Zero)
            {
                return false;
            }

            UIntPtr bytesWritten;
            if (!Win32.WriteProcessMemory(hProcess, intPtr, Encoding.Default.GetBytes(path), num, out bytesWritten))
            {
                return false;
            }

            IntPtr hThread = Win32.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, intPtr, 0U, IntPtr.Zero);
            if (hThread == IntPtr.Zero)
            {
                return false;
            }

            return true;
        }
    }

    internal class Launch
    {
        private static void StartFakeAnticheats()
        {

        }

        public static void LaunchDev() 
        {

            new Thread(() =>
            {

                int Result = HwidBanning.CheckForBan().GetAwaiter().GetResult();

               //tar MessageBox.Show(Result.ToString());

                if (Result == 0x1)
                {
                    MessageBox.Show("You are Currently Banned from Beyond. If this is a mistake, Please go to the support server!");
                    Environment.Exit(0);

                }
                else if (Result == 0x190)
                {
                    MessageBox.Show("An error occured, Please make a ticket in the support server.\n\nError Code: 0x190");
                    Environment.Exit(0);

                }
                else if (Result == 0x180)
                {
                    MessageBox.Show("An error occured, Please make a ticket in the support server.\n\nError Code: 0x180");
                    Environment.Exit(0);

                }

                if (!File.Exists(LoggingSystem.BeyondFolder + "\\FortniteClient-Win64-Shipping_BE.exe"))
                {
                    SimpleUtils.DownloadFile("http://backend.beyondfn.xyz:3551/downloadFakeACBE", LoggingSystem.BeyondFolder + "\\FortniteClient-Win64-Shipping_BE.exe");
                }
                if (!File.Exists(LoggingSystem.BeyondFolder + "\\FortniteLauncher.exe"))
                {
                    SimpleUtils.DownloadFile("http://backend.beyondfn.xyz:3551/downloadFakeACLAUNCHER", LoggingSystem.BeyondFolder + "\\FortniteLauncher.exe");
                }
                Process.Start(new ProcessStartInfo
                {
                    FileName = LoggingSystem.BeyondFolder + "\\FortniteLauncher.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                Process.Start(new ProcessStartInfo
                {
                    FileName = LoggingSystem.BeyondFolder + "\\FortniteClient-Win64-Shipping_BE.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                if (Directory.Exists(Settings.Default.Path + "\\EasyAntiCheat"))
                {
                    Directory.Delete(Settings.Default.Path + "\\EasyAntiCheat", true);
                }

                foreach (var File in Directory.GetFiles(Settings.Default.Path))
                {
                    MessageBox.Show(File);
                }

                if (File.Exists(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                {
                    File.Delete(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                }


               /* File.WriteAllBytes(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll", Resources.Beyond_Client_Dev);

                while (!File.Exists(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                {
                    Task.Delay(500);
                }
*/
                string keyPath = "Software\\NovaFn";
                var SubKey = Registry.CurrentUser.OpenSubKey(keyPath, true);
                SubKey.SetValue("accountId", Resources.ClientVersion);
                SubKey.Close();



                var Proc = Process.Start(new ProcessStartInfo
                {
                    FileName = Settings.Default.Path + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe",
                    Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck -AUTH_TYPE=EPIC -AUTH_LOGIN=" + Settings.Default.Email + " -AUTH_PASSWORD=" + Settings.Default.Password,
               
                    UseShellExecute = false
                });



              //  Win32.Inject(Proc!.Id, Directory.GetCurrentDirectory() + "\\Redirect.dll");

            }).Start();
            
        }

        public static void LaunchGame()
        {
            new Thread(() =>
            {
                try
                {

                    int Result = HwidBanning.CheckForBan().GetAwaiter().GetResult();
                    if (Result == 0x1)
                    {
                        MessageBox.Show("You are Currently Banned from Beyond. If this is a mistake, Please go to the support server!");
                        Environment.Exit(0);
                    }
                    else if (Result == 0x190)
                    {
                        MessageBox.Show("An error occured, Please make a ticket in the support server.\n\nError Code: 0x190");
                        Environment.Exit(0);
                    }
                    else if (Result == 0x180)
                    {
                        MessageBox.Show("An error occured, Please make a ticket in the support server.\n\nError Code: 0x180");
                        Environment.Exit(0);
                    }

                    SimpleUtils.SafeKillProcess("EpicGamesLauncher");
                    SimpleUtils.SafeKillProcess("EpicWebHelper");
                    SimpleUtils.SafeKillProcess("CrashReportClient");
                    SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping_BE");
                    SimpleUtils.SafeKillProcess("FortniteLauncher");
                    SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping");
                    SimpleUtils.SafeKillProcess("EasyAntiCheat_EOS");
                    SimpleUtils.SafeKillProcess(EAC.NewFileName);

                    if (!File.Exists(LoggingSystem.BeyondFolder + "\\FortniteClient-Win64-Shipping_BE.exe"))
                    {
                        SimpleUtils.DownloadFile("http://backend.beyondfn.xyz:3551/downloadFakeACBE", LoggingSystem.BeyondFolder + "\\FortniteClient-Win64-Shipping_BE.exe");
                    }
                    if (!File.Exists(LoggingSystem.BeyondFolder + "\\FortniteLauncher.exe"))
                    {
                        SimpleUtils.DownloadFile("http://backend.beyondfn.xyz:3551/downloadFakeACLAUNCHER", LoggingSystem.BeyondFolder + "\\FortniteLauncher.exe");
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = LoggingSystem.BeyondFolder + "\\FortniteLauncher.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });

                    while (Process.GetProcessesByName("FortniteLauncher").Length == 0)
                    {
                        Thread.Sleep(500);
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = LoggingSystem.BeyondFolder + "\\FortniteClient-Win64-Shipping_BE.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    });

                    if (Directory.Exists(Settings.Default.Path + "\\EasyAntiCheat"))
                    {
                        Directory.Delete(Settings.Default.Path + "\\EasyAntiCheat", true);
                    }
                    foreach (var FileName in Directory.GetFiles(Settings.Default.Path, "*.exe", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            File.Delete(FileName);
                        } catch (Exception ex) {
                          
                        }
                        
                    }
                    if (File.Exists(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                    {
                        File.Delete(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                    }

                    string pak;
                    if (Anticheat.Scan(out pak))
                    {
                        MessageBox.Show($"Cheating Detected! Pak Detected: {pak}");
                        return;
                    }

                    File.WriteAllBytes(Settings.Default.Path + "\\EAC.zip", Resources.EAC);

                    while (!File.Exists(Settings.Default.Path + "\\EAC.zip"))
                    {
                        Task.Delay(500);
                    }

                    ZipFile.ExtractToDirectory(Settings.Default.Path + "\\EAC.zip", Settings.Default.Path);

                    File.WriteAllBytes(Properties.Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll", Resources.Beyond_Client);

                    while (!File.Exists(Properties.Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                    {
                        Task.Delay(500);
                    }

                    string keyPath = "Software\\NovaFn";
                    var SubKey = Registry.CurrentUser.OpenSubKey(keyPath, true);
                    SubKey.SetValue("accountId", Resources.ClientVersion);
                    SubKey.Close();
                    
        
                    Process SetupProc = new();
                    SetupProc.StartInfo = new(Settings.Default.Path + "\\EasyAntiCheat\\EasyAntiCheat_EOS_Setup.exe");
                    SetupProc.StartInfo.Arguments = "install \"257a9a9d2e4d4bc59c14e46d248b9cc0\"";
                    SetupProc.StartInfo.UseShellExecute = false;
                    SetupProc.Start();
                    SetupProc.WaitForExit();

                    // troll
                    EAC.InitEAC();

                    LoggingSystem.WriteToLog("Started EAC and Game..");

            

                    //Thread.Sleep(120000);

                    //// i love eac
                    //while (true)
                    //{
                    //    Thread.Sleep(500);

                    //    Process[] Procs = Process.GetProcessesByName("FortniteClient-Win64-Shipping");

                    //    if (Procs.Length > 0)
                    //    {
                    //        try
                    //        {
                    //            foreach (var Proc in Procs)
                    //            {
                    //                proc.Kill();
                    //            }
                    //        }
                    //        catch { }

                    //        break;
                    //    }
                    //}



                } catch (Exception ex)
                {
                    MessageBox.Show("An Error occured, Error data has been saved to the Logs located in Documents");
                    LoggingSystem.WriteToLog(ex.ToString());
                }
   

            }).Start();
        }
    }
}
