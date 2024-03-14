using BeyondLauncherV2.Properties;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeyondLauncherV2.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tosflyout.FontSize = 15;
            
            if (TosAccept.IsChecked == false)
            {
                tosflyout.Show();
                return;
            }

            using (WebClient wc = new WebClient())
            {
                string res = wc.DownloadString($"http://backend.beyondfn.xyz:8990/loginLauncherAuth/{Email.Text}/{Password.Password}");

                if (res != "yay")
                {
                    tosflyout.Content = "Email/Password is Incorrect!";
                    tosflyout.Show();
                }
                else
                {
                    Settings.Default.Email = Email.Text;
                    Settings.Default.Password = Password.Password;
                    Settings.Default.Save();
                    Globals.NavFrame.Navigate(new HomePage());
                }
            }
        }
    }
    
}
