using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Total_Print.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }
        private void Button_BackClick(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as Views.Navigation).OpenDashboard();
        }
        private void Button_AddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\TotalPrint");
                key.SetValue("", "Print PDF files in the folder");
                key.SetValue("Icon", System.Reflection.Assembly.GetExecutingAssembly().Location);
                key.Close();

                key = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\TotalPrint\command");
                key.SetValue("", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" \"%1\"");
                key.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_RemoveClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
