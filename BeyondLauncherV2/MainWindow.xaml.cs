using BeyondLauncherV2.Pages;
using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using System.Windows;

namespace BeyondLauncherV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoggingSystem.OpenLog();
            _NavigationFrame.Navigate(new HomePage());

            if (Settings.Default.StartRPC)
                RPC.StartRPC();

        }

        private void NavigationItem_Click(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Navigate(new HomePage());
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
            _NavigationFrame.Navigate(new SettingsPage());
        }
    }
}