
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Net;

void Log(string message, ConsoleColor ForeGroundColor = ConsoleColor.White)
{
    Console.ForegroundColor = ForeGroundColor;
    Console.WriteLine(message);
    Console.ForegroundColor = ConsoleColor.White;
}

static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
{
    if (e.TotalBytesToReceive <= 0)
    { return; }
    int percentage = (int)((double)e.BytesReceived / e.TotalBytesToReceive * 100);

    Console.Write("\rProgress: [{0}{1}] {2}%   ",
        new string('#', percentage / 5), new string(' ', 20 - percentage / 5), percentage);

    if (percentage == 100)
    {
        Console.WriteLine();
    }
}

void DownloadFile(string URL, string OutFilePath)
{
    try
    {
        using (WebClient wc = new())
        {
            wc.DownloadProgressChanged += ProgressChanged;

            wc.DownloadFileAsync(new Uri(URL), OutFilePath);

            while (wc.IsBusy)
            {
                Thread.Sleep(100);
            }
        }
    } catch
    {
        Log($"[!] An error occurred while downloading", ConsoleColor.Red);
    }

 
}

Log(@"
  ____  ________     ______  _   _ _____  
 |  _ \|  ____\ \   / / __ \| \ | |  __ \ 
 | |_) | |__   \ \_/ / |  | |  \| | |  | |
 |  _ <|  __|   \   /| |  | | . ` | |  | |
 | |_) | |____   | | | |__| | |\  | |__| |
 |____/|______|  |_|  \____/|_| \_|_____/ 
                                          
                                          
", ConsoleColor.Blue);

Process process = new Process();
process.StartInfo.FileName = "dotnet";
process.StartInfo.Arguments = "--version";
process.StartInfo.UseShellExecute = false;
process.StartInfo.RedirectStandardOutput = true;
process.Start();

string output = process.StandardOutput.ReadToEnd();
process.WaitForExit();

// just because this looked horrible inside the function
const string NetInstallerLink = "https://download.visualstudio.microsoft.com/download/pr/1e9f5038-d1c3-4219-94e9-62d6f810e589/c28bb8c8a1a3d01c72f3db1646a983c5/dotnet-sdk-8.0.204-win-x64.exe";

int Ver;
string FirstLetter = output.First().ToString();

Int32.TryParse(FirstLetter, out Ver);

if (Ver < 8)
{
    back:
    Log("[!] You must have .NET 8 Installed to Use Beyond. Do you want to install it? (Y/N)", ConsoleColor.Blue);

    Console.Write("\n>> ");
    string Input = Console.ReadLine()!;

    if (Input.ToLower() == "y")
    {
        string NetInstallerDir = Directory.GetCurrentDirectory() + "\\NETInstallerBynd.exe";

        DownloadFile(NetInstallerLink, NetInstallerDir);

        var InstallerProc = Process.Start(NetInstallerDir);
        InstallerProc.WaitForExit();

        Console.WriteLine();

        goto install_launcher;
    } 
    else if (Input.ToLower() == "n") 
    {
        Environment.Exit(0);
    } 
    else
    {
        goto back;
    }
}

install_launcher:

Log("[+] Installing Beyond Launcher", ConsoleColor.Green);

DownloadFile("http://backend.beyondfn.xyz:3551/launcherdl", Directory.GetCurrentDirectory() + "\\Beyond Launcher.exe");

Process.Start(Directory.GetCurrentDirectory() + "\\Beyond Launcher.exe");

Log("[+] Beyond Launcher has been installed! Closing in 3 Seconds.", ConsoleColor.Green);

Thread.Sleep(3000);

Environment.Exit(0);