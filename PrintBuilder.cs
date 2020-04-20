using System.IO;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Total_Print
{
    public struct DocFile
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string path { get; set; }
        public bool isSelected { get; set; }
    }

    class PrintBuilder
    {
        private DocFile[] _docsList;
        private int _taskCount;
        private int _taskCur;
        private Action _funcOnDone;

        private PrinterSettings _printer;
        private PageSettings _page;

        public PrintBuilder SetPrinter(string printer)
        {
            this._printer = new PrinterSettings
            {
                PrinterName = printer,
                Copies = (short)1,
            };
            this._page = new PageSettings(this._printer)
            {
                Margins = new Margins(0, 0, 0, 0),
            };

            return this;
        }
        public PrintBuilder SetList(DocFile[] files)
        {
            this._docsList = files;
            this._taskCount = files.Length;
            return this;
        }
        public void Done(Action action)
        {
            _funcOnDone = action;
        }
        public bool Ready()
        {
            if (this._docsList != null && this._printer != null && this._page != null)
                return true;

            if (_funcOnDone != null)
                App.Current.Dispatcher.Invoke(_funcOnDone);
            return false;
        }
        public async void PrintAsync()
        {
            this._taskCur = 0;

            if (this.Ready())
            {
                foreach (DocFile file in _docsList)
                {
                    if (File.Exists(file.path) && file.isSelected)
                        await Task.Run(() => PrintFile(file.path, file.name));
                }
            }
        }
        private void PrintFile(string path, string name = "Unknown PDF")
        {
            Thread.Sleep(100);
            _taskCur++;

            foreach (PaperSize paperSize in this._printer.PaperSizes)
            {
                if (paperSize.PaperName == name)
                {
                    this._page.PaperSize = paperSize;
                    break;
                }
            }

            using (var document = PdfiumViewer.PdfDocument.Load(path))
            {
                using (PrintDocument printDocument = document.CreatePrintDocument())
                {
                    printDocument.PrinterSettings = this._printer;
                    printDocument.DefaultPageSettings = this._page;
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                }
            }

            if(_taskCur == _taskCount)
            {
                if(_funcOnDone != null)
                    App.Current.Dispatcher.Invoke(_funcOnDone);
            }
        }
    }
}
