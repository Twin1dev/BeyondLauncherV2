using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using EasyInstallerV2;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using static EasyInstallerV2.Installer;

namespace BeyondLauncherV2.Pages
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : Page
    {
        public DownloadPage()
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

        }

        DispatcherTimer timer;
        string PathToDownloadTo = "";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadButton.Content == "Cancel")
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }


            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = "Select your Fortnite Folder!";
                dialog.IsFolderPicker = true;
                dialog.EnsurePathExists = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    PathToDownloadTo = dialog.FileName;
                }
                else
                {
                    return;
                }

            }

            var httpClient = new WebClient();

            var manifest = JsonConvert.DeserializeObject<ManifestFile>(httpClient.DownloadString("https://manifest.fnbuilds.services/12.41/12.41.manifest"));

            Download(manifest, "12.41", PathToDownloadTo);

            DownloadButton.Content = "Cancel";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += ProgTick;
            timer.Start();


        }


        private void ProgTick(object sender, EventArgs e)
        {
            ProgText.Content = PROGRESS;
            ProgRing.Progress = PERCENT;

            if (PERCENT >= 99.80)
            {
                timer.Stop();
                DownloadButton.Content = "Done!";
            }
        }

    }
}
