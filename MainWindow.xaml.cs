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

        public void OnDone()
        {
            progressBar.Visibility = Visibility.Hidden;
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
            for (int i = 0; i < fileEntries.Length; i++)
            {
                if (System.IO.Path.GetExtension(fileEntries[i]) == ".pdf")
                    docsList.Add(new DocFile() { id = i, name = System.IO.Path.GetFileName(fileEntries[i]), type = System.IO.Path.GetExtension(fileEntries[i]), path = fileEntries[i], isSelected = true });
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

            printer.Done(OnDone);
        }
        private void CheckBox_Change(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            int id = Convert.ToInt32(check.Tag.ToString());
            
            docsList[id] = new DocFile() {
                name = docsList[id].name,
                path = docsList[id].path,
                type = docsList[id].type,
                isSelected = (bool)check.IsChecked
            };
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
    }
}
