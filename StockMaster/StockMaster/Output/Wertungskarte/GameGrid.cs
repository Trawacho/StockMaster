using StockMaster.BaseClasses;
using StockMaster.Converters;
using System.Windows.Media;
using System.Linq;

namespace StockMaster.Output.Wertungskarte
{
    public class GameGrid : SpiegelGrid
    {
        public GameGrid(Game game, int startNumber, bool kehren8) : base(kehren8)
        {
            int colCounter = kehren8 ? 27 : 25;
            string NumberOfGame = game.GameNumberOverAll.ToString();
            string NumberOfArea = game.CourtNumber.ToString();
            string Opponent = startNumber == game.TeamA.StartNumber ? game.TeamB.StartNumber.ToString() : game.TeamA.StartNumber.ToString();
            //string StartOfGame = game.StartOfPlayTeamA ? game.TeamB.StartNumber.ToString() : game.TeamA.StartNumber.ToString();
            string StartOfGame = game.StartingTeam.StartNumber.ToString();

            RowDefinitions.Add(new System.Windows.Controls.RowDefinition()
            {
                Height = new System.Windows.GridLength(PixelConverter.CmToPx(0.6))
            });

            if (game.IsPauseGame)
            {
                //In Spalte 14(15) die Spielnummer eintragen
                var b1 = new SpiegelFeld(NumberOfGame, 0);
                if (kehren8)
                {
                    SetColumn(b1, 15);
                }
                else
                {
                    SetColumn(b1, 14);
                }
                Children.Add(b1);

                //von Spalte 0 bis 2 "aussetzen" eintragen
                var b2 = new SpiegelFeld("aussetzen", 0);
                SetColumn(b2, 0);
                SetColumnSpan(b2, 3);
                Children.Add(b2);

                for (int i = 3; i < colCounter; i++) //In die Spalten 3 bis 24 einen grauen block eintragen
                {
                    if (kehren8)
                    {
                        if (i == 14 || i == 15) continue; //Die Spalte 13 (Trennstrich) und Spalte 14 (Spielnummer) freilassen

                    }
                    else
                    {
                        if (i == 13 || i == 14) continue; //Die Spalte 13 (Trennstrich) und Spalte 14 (Spielnummer) freilassen
                    }
                    var b = new SpiegelFeld(Brushes.LightGray);
                    SetColumn(b, i);
                    Children.Add(b);
                }
            }
            else
            {
                for (int i = 0; i < colCounter; i++)
                {
                    //leerer Spalt übersrpingen
                    //if (i == 13)
                    if (i == (kehren8 ? 14 : 13))
                        i++;

                    string t;

                    switch (i)
                    {
                        case 0:
                            t = NumberOfArea;
                            break;

                        case 1:
                            t = Opponent;
                            break;

                        case 2:
                            t = StartOfGame;
                            break;

                        case 14:
                            t = !kehren8 ? NumberOfGame : "";
                            break;

                        case 15:
                            t = kehren8 ? NumberOfGame : "";
                            break;

                        default:
                            t = string.Empty;
                            break;
                    }

                    var b = new SpiegelFeld(t, 0);

                    SetColumn(b, i);
                    Children.Add(b);
                }
            }
        }
    }
}
