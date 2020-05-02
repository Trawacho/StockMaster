using System.Windows;
using System.Windows.Documents;

namespace StockMaster.Output
{
    /// <summary>
    /// Interaktionslogik für DocumentViewer.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        public PrintPreview()
        {
            InitializeComponent();
        }
        public IDocumentPaginatorSource Document
        {
            get { return _viewer.Document; }
            set { _viewer.Document = value; }
        }
    }
}
