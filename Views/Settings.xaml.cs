using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
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

            if((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
            }
            else
            {
                addContextButton.IsEnabled = removeContextButton.IsEnabled = false;
                restartStack.Visibility = Visibility.Visible;
            }
        }
        private void Button_RestartClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName) { Verb = "runas", Arguments = "restart" });
            Application.Current.Shutdown();
        }
        private void Button_BackClick(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as Views.Navigation).OpenDashboard();
        }
        private void Button_AddClick(object sender, RoutedEventArgs e)
        {
                RegistryKey key = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\TotalPrint");
                key.SetValue("", "Print PDF files in the folder");
                key.SetValue("Icon", System.Reflection.Assembly.GetExecutingAssembly().Location);
                key.Close();

                key = Registry.ClassesRoot.CreateSubKey(@"Directory\shell\TotalPrint\command");
                key.SetValue("", "explorer.exe totalprint:%1");
                key.Close();
        }
        private void Button_RemoveClick(object sender, RoutedEventArgs e)
        {
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"Directory\shell\TotalPrint\command", true))
            {
                if (key != null)
                {
                    key.DeleteValue("");
                }
            }
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"Directory\shell\TotalPrint", true))
            {
                if (key != null)
                {
                    key.DeleteValue("");
                    key.DeleteValue("Icon");
                }
            }
        }
    }
}
