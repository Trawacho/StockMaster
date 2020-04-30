using StockMaster.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockMaster.Output
{
    public class GameGrid : SpiegelGrid
    {
        public string SpielNummer { get; set; }
        public string Bahn { get; set; }
        public string Anspiel { get; set; }
        public string Gegner { get; set; }

        public GameGrid(Game game, int startNumber)
        {
            string NumberOfGame = game.GameNumber.ToString();
            string NumberOfArea = game.CourtNumber.ToString();
            string Opponent = startNumber == game.TeamA.StartNumber ? game.TeamB.StartNumber.ToString() : game.TeamA.StartNumber.ToString();
            string StartOfGame = game.StartOfPlayTeam1 ? game.TeamB.StartNumber.ToString() : game.TeamA.StartNumber.ToString();

            RowDefinitions.Add(new System.Windows.Controls.RowDefinition()
            {
                Height = new System.Windows.GridLength(pxConverter.CmToPx(0.6))
            });

            if (game.IsPauseGame)
            {
                //In Spalte 14 die Spielnummer eintragen
                var b1 = new SpiegelFeld(game.GameNumber.ToString(), 0);
                SetColumn(b1, 14);
                Children.Add(b1);

                //von Spalte 0 bis 2 "aussetzen" eintragen
                var b2 = new SpiegelFeld("aussetzen", 0);
                SetColumn(b2, 0);
                SetColumnSpan(b2, 3);
                Children.Add(b2);

                for (int i = 3; i < 25; i++) //In die Spalten 3 bis 24 einen grauen block eintragen
                {
                    if (i == 13 || i == 14) continue; //Die Spalte 13 (Trennstrich) und Spalte 14 (Spielnummer) freilassen

                    var b = new SpiegelFeld(Brushes.LightGray);
                    SetColumn(b, i);
                    Children.Add(b);
                }
            }
            else
            {
                for (int i = 0; i < 25; i++)
                {
                    if (i == 13)
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
                            t = NumberOfGame;
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
