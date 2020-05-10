using StockMaster.BaseClasses;
using StockMaster.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMaster.Output.TurnCards
{
    public class Spiegel
    {
        FixedDocument document;
        public Spiegel()
        {

            
        }

        public FixedDocument GetDocument(Size sz, Tournament tournament, bool printTeamName, bool concatRounds)
        {
            document = new FixedDocument();
            document.DocumentPaginator.PageSize = sz;

            var teamPanels = new List<StackPanel>();

            foreach (var team in tournament.Teams.Where(v => !v.IsVirtual)
                                                 .OrderBy(t => t.StartNumber)) //Für jedes Team eine Spiegel-Karte
            {
                if (concatRounds)
                {
                    teamPanels.Add(GetTeamPanel(team, printTeamName));
                }
            }

            var pagePanel = new StackPanel();

            foreach (var teamPanel in teamPanels)
            {
                pagePanel.Children.Add(teamPanel);
                pagePanel.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                pagePanel.Arrange(new Rect(0, 0, pagePanel.DesiredSize.Width, pagePanel.DesiredSize.Height));

                if (pagePanel.ActualHeight + teamPanel.ActualHeight > document.DocumentPaginator.PageSize.Height)
                {
                    SetPagePanelToDocument(pagePanel);
                    pagePanel = new StackPanel();
                }
            }
            if (pagePanel.Children.Count > 0)
            {
                SetPagePanelToDocument(pagePanel);
            }

            return document;
        }

        private void SetPagePanelToDocument(StackPanel panel)
        {
            var newPage = GetNewPage(document.DocumentPaginator.PageSize);

            //Wenn die aktuelle Höhe + die neue Höhe > seiten-Höhe
            FixedPage.SetTop(panel, PixelConverter.CmToPx(1));
            FixedPage.SetLeft(panel, PixelConverter.CmToPx(0.7));
            newPage.Children.Add(panel);

            PageContent content = new PageContent();
            ((IAddChild)content).AddChild(newPage);
            document.Pages.Add(content);
        }

        private FixedPage GetNewPage(Size pageSize)
        {
            FixedPage page = new FixedPage
            {
                Width = pageSize.Width,
                Height = pageSize.Height
            };
            return page;
        }

        private StackPanel GetTeamPanel(Team team, bool printTeamName)
        {
            //alles was eine Karte braucht, kommt in ein StackPanel
            var panel = new StackPanel();

            //Überschhrift-Zeile mit Mannschaften
            panel.Children.Add(new SpiegelHeader(team, printTeamName));
            //Überschriften der Spalten
            panel.Children.Add(new SpiegelHeaderGrid());

            foreach (var game in team.Games.OrderBy(r => r.GameNumberOverAll))
            {
                panel.Children.Add(new GameGrid(game, team.StartNumber));
            }

            //Eine Summenzeile
            panel.Children.Add(new GameSummaryGrid());
            //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
            panel.Children.Add(Tools.CutterLine());
            //Das Panel braucht nach unten noch ein wenig Abstand
            panel.Margin = new Thickness(0, 0, 0, PixelConverter.CmToPx(0.75));

            return panel;
        }


    }


}

