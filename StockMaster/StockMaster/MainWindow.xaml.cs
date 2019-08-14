using StockMaster.Output;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
             t = new Models.TournamentModel();
            t.CreateNewTournament();
            //Output.Spiegel.Printing();


           
        }
        private Models.TournamentModel t;
        private void ButtonPrint_Clicked(object sender, RoutedEventArgs e)
        {

            Output.PrintPreview printPrv = new Output.PrintPreview();
            printPrv.Owner = this;
            printPrv.Document = new Output.Spiegel().Document(new Size(pxConverter.CmToPx(21), pxConverter.CmToPx(29.7)),t.Tournament);
            printPrv.ShowDialog();
        }
    }
}
