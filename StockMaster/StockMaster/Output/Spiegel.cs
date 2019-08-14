using StockMaster.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMaster.Output
{
    public class Spiegel
    {
        //public static void Printing()
        //{
        //    PrintDialog pd = new PrintDialog();
        //    if (pd.ShowDialog() == true)
        //    {
        //        // pd.PrintDocument(doc(new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight)).DocumentPaginator, "My First Print");
        //    }
        //}

        private FixedPage GetNewPage(Size pageSize)
        {
            FixedPage page = new FixedPage();
            page.Width = pageSize.Width;
            page.Height = pageSize.Height;
            return page;
        }

        public FixedDocument Document(Size sz, Tournament tournament)
        {
            var document = new FixedDocument();

            document.DocumentPaginator.PageSize = sz;

            foreach (var team in tournament.Teams)  //Für jedes Team eine Spiegel-Karte
            {
                //alles was eine Karte braucht, kommt in ein StackPanel
                var panel = new StackPanel();   

                //Überschhrift-Zeile mit Mannschaften
                panel.Children.Add(new SpiegelHeader(team));

                //Überschriften der Spalten
                panel.Children.Add(new SpiegelHeaderGrid());   

                foreach (var game in tournament.GetGamesOfTeam(team.StartNumber))   
                {
                    //Jede Spielrunde anfügen
                    panel.Children.Add(new GameGrid(game));
                }

                //Eine Summenzeile
                panel.Children.Add(new GameSummaryGrid());  

                //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
                panel.Children.Add(new Line()
                {
                    X1 = 0,
                    X2 = 1,
                    Y1 = 0,
                    Y2 = 0,
                    Stretch = Stretch.Fill,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Margin = new Thickness(0, pxConverter.CmToPx(0.75), 0, 0)
                });   
                
                //Das Panel braucht nach unten noch ein wenig Abstand
                panel.Margin = new Thickness(0, 0, 0, pxConverter.CmToPx(0.75));

                teamPanels.Add(panel);
            }


            var pagePanel = new StackPanel();
            foreach (var teamPanel in teamPanels)
            {
                pagePanel.Children.Add(teamPanel);
                pagePanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                pagePanel.Arrange(new Rect(0, 0, pagePanel.DesiredSize.Width, pagePanel.DesiredSize.Height));

                if (pagePanel.ActualHeight + teamPanel.ActualHeight > document.DocumentPaginator.PageSize.Height)
                {
                    //Neue Seite
                    var newPage = GetNewPage(document.DocumentPaginator.PageSize);

                    //Wenn die aktuelle Höhe + die neue Höhe > seiten-Höhe
                    FixedPage.SetTop(pagePanel, pxConverter.CmToPx(1));
                    FixedPage.SetLeft(pagePanel, pxConverter.CmToPx(0.7));
                    newPage.Children.Add(pagePanel);

                    PageContent content = new PageContent();
                    ((IAddChild)content).AddChild(newPage);
                    document.Pages.Add(content);

                    pagePanel = new StackPanel();

                }
            }

            return document;
        }

        private List<StackPanel> teamPanels = new List<StackPanel>();


    }


}

