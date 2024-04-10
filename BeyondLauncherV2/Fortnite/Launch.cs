using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;

namespace BeyondLauncherV2.Fortnite
{
    internal class EAC
    {
        // idfk what to name this
        public static void InitEAC()
        {
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
            SubKey.SetValue("accountId", Anticheat.GetFileHash(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"));
            SubKey.Close();

            LoggingSystem.WriteToLog("EAC Installed.");
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


        public static void Inject(int pid, string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Error while Injecting");
                return;
            }

            IntPtr hProcess = Win32.OpenProcess(1082, false, pid);
            IntPtr procAddress = Win32.GetProcAddress(Win32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            uint num = checked((uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))));
            IntPtr intPtr = Win32.VirtualAllocEx(hProcess, IntPtr.Zero, num, 12288U, 4U);
            UIntPtr bytesWritten;
            Win32.WriteProcessMemory(hProcess, intPtr, Encoding.Default.GetBytes(path), num, out bytesWritten);
            Win32.CreateRemoteThread(hProcess, IntPtr.Zero, 0U, procAddress, intPtr, 0U, IntPtr.Zero);
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

                if (HwidBanning.CheckForBan().GetAwaiter().GetResult())
                {
                    MessageBox.Show("You are Currently Banned from Beyond. If this is a mistake, Please go to the support server or Check logs!");
                    Environment.Exit(0);
                    return;
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
                if (File.Exists(Settings.Default.Path + "\\Beyond.exe"))
                {
                    File.Delete(Settings.Default.Path + "\\Beyond.exe");
                }
                if (File.Exists(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                {
                    File.Delete(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                }


                File.WriteAllBytes(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll", Resources.Beyond_Client_Dev);

                while (!File.Exists(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                {
                    Task.Delay(500);
                }

                string keyPath = "Software\\NovaFn";
                var SubKey = Registry.CurrentUser.OpenSubKey(keyPath, true);
                SubKey.SetValue("accountId", Anticheat.GetFileHash(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"));
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
                    if (HwidBanning.CheckForBan().GetAwaiter().GetResult())
                    {
                        MessageBox.Show("You are Currently Banned from Beyond. If this is a mistake, Please go to the support server!");
                        Environment.Exit(0);
                        return;
                    }

                    SimpleUtils.SafeKillProcess("EpicGamesLauncher");
                    SimpleUtils.SafeKillProcess("EpicWebHelper");
                    SimpleUtils.SafeKillProcess("CrashReportClient");
                    SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping_BE");
                    SimpleUtils.SafeKillProcess("FortniteLauncher");
                    SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping");
                    SimpleUtils.SafeKillProcess("EasyAntiCheat_EOS");
                    SimpleUtils.SafeKillProcess("Beyond");

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
                    if (File.Exists(Settings.Default.Path + "\\Beyond.exe"))
                    {
                        File.Delete(Settings.Default.Path + "\\Beyond.exe");
                    }
                    if (File.Exists(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll"))
                    {
                        File.Delete(Settings.Default.Path + "\\Engine\\Binaries\\ThirdParty\\NVIDIA\\NVaftermath\\Win64\\GFSDK_Aftermath_Lib.x64.dll");
                    }

                    string pak;
                    if (Anticheat.Scan(out pak))
                    {
                        MessageBox.Show($"Cheating Detected! Pak Detected: {pak}");
                    }

                    EAC.InitEAC();

                    LoggingSystem.WriteToLog("Opening EOS_Setup");

                    Process SetupProc = new();
                    SetupProc.StartInfo = new(Settings.Default.Path + "\\EasyAntiCheat\\EasyAntiCheat_EOS_Setup.exe");
                    SetupProc.StartInfo.Arguments = "install \"257a9a9d2e4d4bc59c14e46d248b9cc0\"";
                    SetupProc.StartInfo.UseShellExecute = false;
                    SetupProc.Start();
                    SetupProc.WaitForExit();

                    Process proc = new();
                    proc.StartInfo.FileName = Settings.Default.Path + "\\Beyond.exe";
                    proc.StartInfo.Arguments = "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck -AUTH_TYPE=EPIC -AUTH_LOGIN=" + Settings.Default.Email + " -AUTH_PASSWORD=" + Settings.Default.Password;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();

                    LoggingSystem.WriteToLog("Started EAC and Game..");
                } catch (Exception ex)
                {
                    MessageBox.Show("An Error occured, Error data has been saved to the Logs located in Documents");
                    LoggingSystem.WriteToLog(ex.ToString());
                }
   

            }).Start();
        }
    }
}
