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

namespace StockMaster.Output.Wertungskarte
{
    public class Wertungskarte
    {
        FixedDocument document;
        public Wertungskarte()
        {

        }

        public FixedDocument GetDocument(Size pageSize, Tournament tournament, bool printTeamName, bool concatRounds)
        {
            document = new FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;

            var teamPanels = new List<StackPanel>();

            foreach (var team in tournament.Teams.Where(v => !v.IsVirtual)
                                                 .OrderBy(t => t.StartNumber)) //Für jedes Team eine Spiegel-Karte
            {
                if (concatRounds)
                {
                    teamPanels.Add(GetTeamPanel(team, printTeamName, tournament.Is8KehrenSpiel));
                }
                else
                {
                    teamPanels.AddRange(GetTeamPanels(team, printTeamName, tournament.Is8KehrenSpiel));
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

        /// <summary>
        /// Get Panels from a Team for each Round
        /// </summary>
        /// <param name="team"></param>
        /// <param name="printTeamName"></param>
        /// <param name="kehren8"></param>
        /// <returns></returns>
        private List<StackPanel> GetTeamPanels(Team team, bool printTeamName, bool kehren8)
        {
            var numberOfRounds = team.Games.Max(r => r.RoundOfGame);
            //alles was eine Karte braucht, kommt in ein StackPanel
            var panels = new List<StackPanel>();

            for (int round = 1; round <= numberOfRounds; round++)
            {
                //alles was eine Karte braucht, kommt in ein StackPanel
                var panel = new StackPanel();

                //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
                panel.Children.Add(Tools.CutterLineTop());

                //Überschhrift-Zeile mit Mannschaften
                panel.Children.Add(new SpiegelHeader(team, printTeamName, kehren8, round));

                //Überschriften der Spalten
                panel.Children.Add(new SpiegelHeaderGrid(kehren8));

                foreach (var game in team.Games.Where(g => g.RoundOfGame == round).OrderBy(r => r.GameNumber))
                {
                    panel.Children.Add(new GameGrid(game, team.StartNumber, kehren8));
                }

                //Eine Summenzeile
                panel.Children.Add(new GameSummaryGrid(kehren8));
                //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
                panel.Children.Add(Tools.CutterLine());

                panels.Add(panel);

            }

            return panels;
        }

        /// <summary>
        /// Get Panel from a Team with all Games
        /// </summary>
        /// <param name="team"></param>
        /// <param name="printTeamName"></param>
        /// <param name="kehren8"></param>
        /// <returns></returns>
        private StackPanel GetTeamPanel(Team team, bool printTeamName, bool kehren8)
        {
            //alles was eine Karte braucht, kommt in ein StackPanel
            var panel = new StackPanel();


            //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
            panel.Children.Add(Tools.CutterLineTop());

            //Überschhrift-Zeile mit Mannschaften
            panel.Children.Add(new SpiegelHeader(team, printTeamName, kehren8));
            //Überschriften der Spalten
            panel.Children.Add(new SpiegelHeaderGrid(kehren8));

            foreach (var game in team.Games.OrderBy(r => r.GameNumberOverAll))
            {
                panel.Children.Add(new GameGrid(game, team.StartNumber, kehren8));
            }

            //Eine Summenzeile
            panel.Children.Add(new GameSummaryGrid(kehren8));
            //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
            panel.Children.Add(Tools.CutterLine());

            return panel;
        }


    }


}

