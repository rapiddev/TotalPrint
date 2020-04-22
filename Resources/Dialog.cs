using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Total_Print.Resources
{
    class Dialog
    {
        private string _path = "";
        public Dialog(string name = "Select directory", string startDir = null)
        {
            if (startDir == null)
                startDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString();

            CommonOpenFileDialog _dialog = new CommonOpenFileDialog()
            {
                Title = name,
                IsFolderPicker = true,
                InitialDirectory = startDir,
                DefaultDirectory = startDir,
                AddToMostRecentlyUsedList = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            if (_dialog.ShowDialog() == CommonFileDialogResult.Ok)
                this._path = _dialog.FileName;
            else
                this._path = startDir;
        }
        public string Get()
        {
            return this._path;
        }
    }
}
