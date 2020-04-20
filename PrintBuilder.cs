using System.IO;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Threading;

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
        DocFile[] docsList;

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
            this.docsList = files;
            return this;
        }
        public object OnDone(object t)
        {
            return t;
        }
        public bool Ready()
        {
            if (this.docsList != null && this._printer != null && this._page != null)
                return true;
            return false;
        }
        public async void PrintAsync()
        {
            if(this.Ready())
            {
                foreach (DocFile file in docsList)
                {
                    if (File.Exists(file.path) && file.isSelected)
                        await Task.Run(() => PrintFile(file.path, file.name));
                }
            }
        }
        private void PrintFile(string path, string name = "Unknown PDF")
        {
            Thread.Sleep(100);

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
        }
    }
}
