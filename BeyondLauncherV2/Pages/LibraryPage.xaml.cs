using BeyondLauncherV2.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;
using BeyondLauncherV2.Utilities;
using Wpf.Ui.Common;

namespace BeyondLauncherV2.Pages
{
    public partial class LibraryPage : Page
    {
        public LibraryPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(.5)
            };

            DoubleAnimation slideDownAnimation = new DoubleAnimation
            {
                From = -20,
                To = 0,
                Duration = TimeSpan.FromSeconds(.7)
            };

            TranslateTransform translateTransform = new TranslateTransform();
            MainGrid.RenderTransform = translateTransform;

            MainGrid.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            translateTransform.BeginAnimation(TranslateTransform.XProperty, slideDownAnimation);

            if (Settings.Default.Path != "")
            {
                string[] processesToCheck = { "EasyAntiCheat_EOS", "FortniteClient-Win64-Shipping", "FortniteLauncher", "FortniteClient-Win64-Shipping_BE" };

                bool anyProcessRunning = false;

                foreach (string processName in processesToCheck)
                {
                    Process[] processes = Process.GetProcessesByName(processName);
                    if (processes.Length > 0)
                    {
                        anyProcessRunning = true;
                        break;
                    }
                }

                if (anyProcessRunning)
                {
                    LocateButton1.Content = "Close";
                    LocateButton1.Icon = Wpf.Ui.Common.SymbolRegular.ErrorCircle24;

                }
                else
                {
                    LocateButton1.Content = "Launch";
                    LocateButton1.Icon = Wpf.Ui.Common.SymbolRegular.Play24;
                }
            }
            else
            {
                LocateButton1.Content = "Select Path";
                LocateButton1.Icon = Wpf.Ui.Common.SymbolRegular.Folder24;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Wpf.Ui.Controls.Button)sender;
            if (button.Content.ToString() == "Launch")
            {
                Globals.NavFrame.Navigate(new HomePage());
                return;
            }
            else if (button.Content.ToString() == "Close")
            {

                SimpleUtils.SafeKillProcess("EpicGamesLauncher");
                SimpleUtils.SafeKillProcess("EpicWebHelper");
                SimpleUtils.SafeKillProcess("CrashReportClient");
                SimpleUtils.SafeKillProcess("FortniteLauncher");
                SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping_BE");
                SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping");
                SimpleUtils.SafeKillProcess("EasyAntiCheat_EOS");
                SimpleUtils.SafeKillProcess("Beyond");
                button.Content = "Launch";
                button.Icon = SymbolRegular.Play24;
                Thread.Sleep(1500);
                SimpleUtils.SafeKillProcess("EpicGamesLauncher");
                SimpleUtils.SafeKillProcess("EpicWebHelper");
                SimpleUtils.SafeKillProcess("CrashReportClient");
                SimpleUtils.SafeKillProcess("FortniteLauncher");
                SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping_BE");
                SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping");
                SimpleUtils.SafeKillProcess("EasyAntiCheat_EOS");
                SimpleUtils.SafeKillProcess("Beyond");
                return;
            }
            else if (button.Content.ToString() == "Select Path") 
            {
                using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Select your Fortnite Folder!";
                    dialog.IsFolderPicker = true;
                    dialog.EnsurePathExists = true;
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        if (!Directory.Exists(dialog.FileName + "\\FortniteGame"))
                        {
                            System.Windows.MessageBox.Show("This path does not have Fortnite!");
                            return;
                        }
                        else
                        {
                            Properties.Settings.Default.Path = dialog.FileName;
                            Properties.Settings.Default.Save();
                        }
                    }
                }
            }
        }
 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Downloading is disabled until a future update. Please use EasyInstallerV2 instead. (In #downloads)");
        }
    }
}
