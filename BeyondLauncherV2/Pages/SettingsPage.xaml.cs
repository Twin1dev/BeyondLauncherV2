using BeyondLauncherV2.Properties;
using BeyondLauncherV2.Utilities;
using System;
using System.Collections.Generic;
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
using Wpf.Ui.Controls;

namespace BeyondLauncherV2.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
           
        }

        private void RPCSwitch_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void RPCSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void RPCSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            RPCSwitch.IsChecked = Settings.Default.StartRPC;
        }

        private void RPCSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (RPCSwitch.IsChecked == true)
            {
                Settings.Default.StartRPC = true;
                Settings.Default.Save();
                RPC.StartRPC();
            } else
            {
                Settings.Default.StartRPC = false;
                Settings.Default.Save();

                MessageBoxResult MsgResult = MessageBox.Show("Beyond has to restart in order to stop the rpc, would you like to restart?", "Info", MessageBoxButton.YesNo);

                if (MsgResult == MessageBoxResult.Yes)
                {
                    SimpleUtils.Restart();
                }
            }
           
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

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            MessageBoxResult MsgResult = MessageBox.Show("Beyond has to restart in order to logout, would you like to restart?", "Info", MessageBoxButton.YesNo);

            if (MsgResult == MessageBoxResult.Yes)
            {
                Settings.Default.Reset();
                Settings.Default.Save();

                SimpleUtils.Restart();
            }
        }
    }
}
