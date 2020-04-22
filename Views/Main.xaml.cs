using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Total_Print.Resources;

namespace Total_Print.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private List<DocFile> docsList = new List<DocFile> { };
        public Main()
        {
            InitializeComponent();

            if (!this.Arguments())
            {
                textBoxDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
                if (Directory.Exists(textBoxDirectory.Text))
                    ProcessDirectory(textBoxDirectory.Text);
            }

            if (docsList.Count > 0)
                filesList.ItemsSource = docsList;
        }
        public void OnDone()
        {
            progressBar.Visibility = Visibility.Hidden;
        }
        private bool Arguments()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if(args[1].StartsWith("totalprint:"))
                {
                    string dirBuild = args[1].Remove(0, 11);
                    if(args.Length > 2)
                    {
                        for (int i = 2; i < args.Length; i++)
                        {
                            dirBuild += " " + args[i];
                        }
                    }
                    
                    if(Directory.Exists(dirBuild))
                    {
                        textBoxDirectory.Text = dirBuild;
                        ProcessDirectory(dirBuild);
                        return true;
                    }
                    return false;
                }

                if (Directory.Exists(args[1]))
                {
                    textBoxDirectory.Text = args[1];
                    ProcessDirectory(args[1]);
                    return true;
                }
                return false;
            }
            return false;
        }
        private void ProcessDirectory(string targetDirectory)
        {
            docsList = new List<DocFile> { };
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            for (int i = 0; i < fileEntries.Length; i++)
            {
                if (System.IO.Path.GetExtension(fileEntries[i]) == ".pdf")
                    docsList.Add(new DocFile()
                    {
                        id = i,
                        name = System.IO.Path.GetFileName(fileEntries[i]),
                        type = System.IO.Path.GetExtension(fileEntries[i]),
                        path = fileEntries[i],
                        isSelected = true
                    });
            }
        }
        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string path = new Dialog("Select folder for printing", textBoxDirectory.Text).Get();
            textBoxDirectory.Text = path;

            if (Directory.Exists(path))
            {
                ProcessDirectory(path);
                filesList.ItemsSource = docsList;
            }
        }
        private void Button_PrintClick(object sender, RoutedEventArgs e)
        {
            PrintBuilder printer = new PrintBuilder()
                .SetList(docsList.ToArray())
                .SetPrinter(comboboxPrinter.SelectedItem.ToString());

            if (printer.Ready())
            {
                progressBar.Visibility = Visibility.Visible;
                printer.PrintAsync();
            }

            printer.Done(OnDone);
        }
        private void Button_SettingsClick(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as Views.Navigation).OpenSettings();
        }
        private void CheckBox_Change(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            int id = Convert.ToInt32(check.Tag.ToString());

            for (int i = 0; i < docsList.Count; i++)
            {
                if (docsList[i].id == id)
                {
                    docsList[i] = new DocFile()
                    {
                        name = docsList[i].name,
                        path = docsList[i].path,
                        type = docsList[i].type,
                        isSelected = (bool)check.IsChecked
                    };
                }
            }
        }
    }
}
