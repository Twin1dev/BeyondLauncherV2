using BeyondLauncherV2.Utilities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BeyondLauncherV2.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

        }

        private void Page_Initialized(object sender, EventArgs e)
        {
          
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


    }
}
