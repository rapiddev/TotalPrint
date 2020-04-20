using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Total_Print
{
    public partial class MainWindow : Window
    {
        private List<DocFile> docsList = new List<DocFile> { };
        private string printerName;
        public MainWindow()
        {
            InitializeComponent();
            textBoxDirectory.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();
            if (Directory.Exists(textBoxDirectory.Text))
            {
                ProcessDirectory(textBoxDirectory.Text);
                filesList.ItemsSource = docsList;
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string path = PickFolderDialog(textBoxDirectory.Text, "Select folder for printing");
            textBoxDirectory.Text = path;

            if (Directory.Exists(path))
            {
                ProcessDirectory(path);
                filesList.ItemsSource = docsList;
            }
        }
        private void ProcessDirectory(string targetDirectory)
        {
            docsList = new List<DocFile> { };
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            int i = 0;
            foreach (string fileName in fileEntries)
            {
                if(System.IO.Path.GetExtension(fileName) == ".pdf")
                    docsList.Add(new DocFile() { id = i++, name = System.IO.Path.GetFileName(fileName), type = System.IO.Path.GetExtension(fileName), path = fileName, isSelected = true });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            PrintBuilder printer = new PrintBuilder()
                .SetList(docsList.ToArray())
                .SetPrinter(comboboxPrinter.SelectedItem.ToString());


            if (printer.Ready())
                printer.PrintAsync();

            //printer.OnDone()
            //progressBar.Visibility = Visibility.Hidden;
        }
        private string PickFolderDialog(string _def, string _name = "Select directory")
        {
            string path = "";
            CommonOpenFileDialog _dialog = new CommonOpenFileDialog()
            {
                Title = _name,
                IsFolderPicker = true,
                InitialDirectory = _def,
                DefaultDirectory = _def,
                AddToMostRecentlyUsedList = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            if (_dialog.ShowDialog() == CommonFileDialogResult.Ok)
                path = _dialog.FileName;
            return path;
        }
        private void CheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox check = sender as CheckBox;

            int id = Convert.ToInt32(check.Tag.ToString());
            //docsList[id].isSelected = (bool)check.IsChecked;
        }
    }
}
