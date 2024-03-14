using BeyondLauncherV2.Fortnite;
using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using Microsoft.Win32;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Wpf.Ui.Common;
using System.Diagnostics;

namespace BeyondLauncherV2.Pages
{
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();

            if (Settings.Default.Email != "")
            {
                using (WebClient wc = new WebClient())
                {
                    string username = wc.DownloadString($"http://backend.beyondfn.xyz:8990/getUsernamebyemail/{Settings.Default.Email}");

                    HelloLabel.Content = "Hello, " + username + "!";
                }
            }
            else
            {
                Settings.Default.Save();
                MessageBox.Show("Seems something really odd has happened with your launcher, please restart it.");
            }
        }

        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
          

            if ((string)button.Content == "Set Path")
            {
                Globals.NavFrame.Navigate(new LibraryPage());
                return;
            }

            string keyPath = "Software\\NovaFn";
            var SubKey = Registry.CurrentUser.OpenSubKey(keyPath, true);

            if (SubKey == null)
            {
                MessageBox.Show("Issue while launching. Try again in Admin");
                Registry.CurrentUser.CreateSubKey(keyPath).Close();
                return;
            }

            if ((string)button.Content == "Close")
            {
                try
                {
                    SubKey.DeleteValue("accountId");
                    SubKey.Close();
                }
                catch { }

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

            ToastUtils.ShowToast("Launching Game..", "This may take a bit.");

            DependencyObject parent = VisualTreeHelper.GetParent(this);
            while (!(parent is Window) && parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is Window parentWindow)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }

            LoggingSystem.WriteToLog("Launching Game");

            if (Settings.Default.StartRPC)
                RPC.UpdateRPC("Launching Game", true);

            SimpleUtils.SafeKillProcess("EpicGamesLauncher");
            SimpleUtils.SafeKillProcess("EpicWebHelper");
            SimpleUtils.SafeKillProcess("CrashReportClient");
            SimpleUtils.SafeKillProcess("FortniteLauncher");
            SimpleUtils.SafeKillProcess("FortniteClient-Win64-Shipping");
            SimpleUtils.SafeKillProcess("EasyAntiCheat_EOS");
            SimpleUtils.SafeKillProcess("Beyond");
            Launch.LaunchGame();

            button.Content = "Close";
            button.Icon = SymbolRegular.ErrorCircle24;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            if (Settings.Default.Path == "")
            {
                button.Content = "Set Path";
                button.Icon = Wpf.Ui.Common.SymbolRegular.Folder24;
            }

            string[] processesToCheck = { "EasyAntiCheat_EOS", "Beyond", "FortniteClient-Win64-Shipping", "FortniteLauncher", "FortniteClient-Win64-Shipping_BE" };

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
                button.Content = "Close";
                button.Icon = Wpf.Ui.Common.SymbolRegular.ErrorCircle24;
            }
            else
            {
                button.Content = "Launch";
                button.Icon = Wpf.Ui.Common.SymbolRegular.Play24;
            }
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                ScaleTransform scaleTransform = new ScaleTransform();

                button.RenderTransform = scaleTransform;

                DoubleAnimation animation = new DoubleAnimation
                {
                    To = 1.01,
                    Duration = TimeSpan.FromSeconds(0.05),
                };

                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    To = 0.85,
                    Duration = TimeSpan.FromSeconds(0.2),
                };

                button.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                button.BeginAnimation(Button.OpacityProperty, opacityAnimation);
            }
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                ScaleTransform scaleTransform = new ScaleTransform();

                button.RenderTransform = scaleTransform;

                DoubleAnimation animation = new DoubleAnimation
                {
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.3),
                };

                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    To = 0.75,
                    Duration = TimeSpan.FromSeconds(0.2),
                };

                button.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                button.BeginAnimation(Button.OpacityProperty, opacityAnimation);
            }
        }

       
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SimpleUtils.OpenLink("https://discord.gg/beyondmp");
        }

        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            SimpleUtils.OpenLink("https://beyond-shop.tebex.io");
        }

        #endregion

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
                Duration = TimeSpan.FromSeconds(.8)
            };

            TranslateTransform translateTransform = new TranslateTransform();
            MainGrid.RenderTransform = translateTransform;

            MainGrid.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            translateTransform.BeginAnimation(TranslateTransform.XProperty, slideDownAnimation);

        }


        private void DonateButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DonateButton.FontSize = 18;
        }

        private void DonateButton_MouseLeave(object sender, MouseEventArgs e)
        {
            DonateButton.FontSize = 16;
        }

        private void Discord_MouseEnter(object sender, MouseEventArgs e)
        {
            DiscordButton.FontSize = 18;
        }

        private void Discord_MouseLeave(object sender, MouseEventArgs e)
        {
            DiscordButton.FontSize = 16;
        }

    }
}
