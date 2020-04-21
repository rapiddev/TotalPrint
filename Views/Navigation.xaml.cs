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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Total_Print.Views
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : Window
    {
        Views.Main _main;
        Views.Settings _settings;
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
    }
}
