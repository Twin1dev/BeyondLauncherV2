using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

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

    internal class Launch
    {
        private static void StartFakeAnticheats()
        {

        }

        public static void LaunchGame()
        {
            new Thread(() =>
            {
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
                SetupProc.StartInfo.Arguments = "install \"ef7b6dadbcdf42c6872aa4ad596bbeaf\"";
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

            }).Start();
        }
    }
}
