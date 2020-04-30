using StockMaster.Models;
using StockMaster.Output;
using System.Windows;

namespace StockMaster
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void ButtonRefresh_Clicked(object sender, RoutedEventArgs e)
        {
            var t = this.DataContext as TournamentModel;
            t.RaisePropertyChanged(nameof(t.Ergebnisliste));
        }

        private void ButtonPrint_Clicked(object sender, RoutedEventArgs e)
        {
            var t = this.DataContext as TournamentModel;
            var printPrv = new PrintPreview
            {
                Owner = this,
                Document = new Spiegel().Document(new Size(pxConverter.CmToPx(21), pxConverter.CmToPx(29.7)), t.Tournament)
            };
            printPrv.ShowDialog();
        }
    }
}
