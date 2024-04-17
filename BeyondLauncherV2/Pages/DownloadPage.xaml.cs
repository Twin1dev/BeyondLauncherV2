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

        string PathToDownloadTo = "";
        string fileUrl = "http://104.194.10.244:3550/download/EasyInstallerV2.exe";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadButton.Content.ToString() == "Cancel")
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
            else
            {
                using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
                {
                    dialog.Multiselect = false;
                    dialog.Title = "Select the folder to save the file";
                    dialog.IsFolderPicker = true;
                    dialog.EnsurePathExists = true;
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        PathToDownloadTo = dialog.FileName;
                        DownloadFile();
                    }
                }
            }
        }

        private void DownloadFile()
        {
            if (!string.IsNullOrEmpty(PathToDownloadTo))
            {
                using (WebClient client = new WebClient())
                {
                    string fileName = System.IO.Path.GetFileName(fileUrl);
                    string filePath = System.IO.Path.Combine(PathToDownloadTo, fileName);
                    try
                    {
                        client.DownloadFile(fileUrl, filePath);
                        Process.Start(filePath, PathToDownloadTo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error downloading file: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a folder to download the file to.");
            }
        }
    }
}

