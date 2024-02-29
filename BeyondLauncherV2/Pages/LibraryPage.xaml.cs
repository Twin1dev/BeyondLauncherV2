using BeyondLauncherV2.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace BeyondLauncherV2.Pages
{
    /// <summary>
    /// Interaction logic for LibraryPage.xaml
    /// </summary>
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
                LocateButton1.Content = "Launch";
                LocateButton1.Icon = Wpf.Ui.Common.SymbolRegular.Play24;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Wpf.Ui.Controls.Button)sender;
            if (button.Content == "Launch")
            {
                Globals.NavFrame.Navigate(new HomePage());
                return;
            }

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
                        System.Windows.MessageBox.Show("This path does not have fortnite!");
                        return;
                    }
                    else
                    {
                        Properties.Settings.Default.Path = dialog.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }

            
            button.Content = "Launch";
            button.Icon = Wpf.Ui.Common.SymbolRegular.Play24;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Downloading is disabled until a future update. Please use EasyInstallerV2 instead. (In #downloads)");
        }
    }
}
