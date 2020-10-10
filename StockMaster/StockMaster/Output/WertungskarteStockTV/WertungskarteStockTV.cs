using StockMaster.BaseClasses;
using StockMaster.Output.WertungsKarteBase;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace StockMaster.Output.WertungskarteStockTV
{
    class WertungskarteStockTV  : WertungskarteBase
    {
      
        public WertungskarteStockTV()
        {

        }


        /// <summary>
        /// Get Panels from a Team for each Round
        /// </summary>
        /// <param name="team"></param>
        /// <param name="printTeamName"></param>
        /// <param name="kehren8"></param>
        /// <returns></returns>
        internal override List<StackPanel> GetTeamPanels(Team team, bool printTeamName, bool kehren8)
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
                    //panel.Children.Add(new GameGrid(game, team.StartNumber, kehren8));
                    panel.Children.Add(new GameGrid(game, team, kehren8));
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
        internal override StackPanel GetTeamPanel(Team team, bool printTeamName, bool kehren8)
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
                //panel.Children.Add(new GameGrid(game, team.StartNumber, kehren8));
                panel.Children.Add(new GameGrid(game, team, kehren8));
            }

            //Eine Summenzeile
            panel.Children.Add(new GameSummaryGrid(kehren8));
            //Eine Linie zum Schneiden bzw Trennen zur nächsten Spiegelkarte
            panel.Children.Add(Tools.CutterLine());

            return panel;
        }

    }
}
