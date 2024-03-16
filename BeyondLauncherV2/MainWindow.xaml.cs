using BeyondLauncherV2.Pages;
using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;

namespace BeyondLauncherV2
{

    public partial class MainWindow : Window
    {
        public static ImageBrush imgBrush = new ImageBrush();
        public MainWindow()
        {
            InitializeComponent();
            LoggingSystem.OpenLog();
            
            if (Settings.Default.Email == "" || Settings.Default.Password == "")
            {
                _NavigationFrame.Navigate(new LoginPage());

                RootNavigation.Visibility = Visibility.Hidden;
                FrameForTheGang.Visibility = Visibility.Hidden;
                AvatarButton.Visibility = Visibility.Hidden;
            }
            else
                _NavigationFrame.Navigate(new HomePage());

            if (Settings.Default.StartRPC)
                RPC.StartRPC();

            Globals.NavFrame = _NavigationFrame;

          
            imgBrush.ImageSource = new BitmapImage(new Uri(SimpleUtils.GetRandomSkinLink()));

            AvatarButton.Background = imgBrush;
        }

        private void NavigationItem_Click(object sender, RoutedEventArgs e)
        {
            if (!(_NavigationFrame.Content is HomePage))
            {
                _NavigationFrame.Navigate(new HomePage());

                try
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri(RPC.client.CurrentUser.GetAvatarURL(DiscordRPC.User.AvatarFormat.PNG)));

                    AvatarButton.Background = imageBrush;
                }
                catch { }
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
              RPC.StopRPC();
            } catch { } 
        }


        private void NavigationItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (!(_NavigationFrame.Content is SettingsPage))
            {
                _NavigationFrame.Navigate(new SettingsPage());
            }
        }

        private void NavigationItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (!(_NavigationFrame.Content is LibraryPage))
            {
                _NavigationFrame.Navigate(new LibraryPage());
            }
        }

        private void _NavigationFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        { 
            Page nextPage = e.Content as Page;
            if (nextPage != null)
            {
                if (nextPage.ToString().Contains("HomePage"))
                {
                    RootNavigation.Visibility = Visibility.Visible;
                    FrameForTheGang.Visibility = Visibility.Visible;
                    AvatarButton.Visibility = Visibility.Visible;
                }
            }
        }


    }
}