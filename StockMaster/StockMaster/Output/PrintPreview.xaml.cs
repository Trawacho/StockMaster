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
