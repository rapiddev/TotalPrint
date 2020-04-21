using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Total_Print.Views
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : Window
    {
        Views.Main _main;
        Views.Settings _settings;

        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;
        private Duration _duration = new Duration(TimeSpan.FromSeconds(1));
        private double _oldHeight = 0;
        public Navigation()
        {
            InitializeComponent();

            this.OpenDashboard();
        }
        public void OpenDashboard()
        {
            if (_main == null)
                _main = new Main();
            rootFrame.Navigate(_main);
        }
        public void OpenSettings()
        {
            if (_settings == null)
                _settings = new Settings();
            rootFrame.Navigate(_settings);
        }
        private void rootOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            ThicknessAnimation tAnim = new ThicknessAnimation() {
                Duration = TimeSpan.FromSeconds(0.3),
                DecelerationRatio = 0.7,
                To = new Thickness(0, 0, 0, 0)
            };
            if (e.NavigationMode == NavigationMode.New)
                tAnim.From = new Thickness(0, 100, 0, 0);
            else if (e.NavigationMode == NavigationMode.Back)
                tAnim.From = new Thickness(0, 0, 0, 100);

            (e.Content as Page).BeginAnimation(MarginProperty, tAnim);
        }
    }
}
