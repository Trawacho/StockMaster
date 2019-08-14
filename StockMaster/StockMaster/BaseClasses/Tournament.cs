using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Tournament
    {
        /// <summary>
        /// Liste aller Teams
        /// </summary>
        public Teams Teams { get; set; }

        /// <summary>
        /// Liste aller Spiele
        /// </summary>
        public List<Game> Games { get; set; }

        /// <summary>
        /// Veranstaltungsort
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Organisator / Veranstalter
        /// </summary>
        public string Organizer { get; set; }

        /// <summary>
        /// Datum / Zeit des Turniers
        /// </summary>
        public DateTime DateOfTournament { get; set; }


        /// <summary>
        /// Durchführer
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Turniername
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Anzahl der Stockbahnen / Spielfächen
        /// </summary>
        public int CountOfAreas { get; set; }

        public Tournament()
        {
            this.Teams = new Teams();
            this.Games = new List<Game>();
        }

        public List<Game> GetGamesOfTeam(int StartNumber)
        {
            List<Game> games = new List<Game>();

            var x = Games.Where(k => k.Team1.StartNumber == StartNumber || k.Team2.StartNumber == StartNumber);

            foreach (var g in x.Where(k => k.Team2.StartNumber == StartNumber))
            {
                //Die Startnummer von Team2 mit Team1 tauschen. Das Anpsiel muss dann auch getauscht werden
                var t1 = g.Team2;
                g.Team2 = g.Team1;
                g.Team1 = t1;
                if (g.StartOfPlayTeam1)
                    g.StartOfPlayTeam1 = false;
                else
                    g.StartOfPlayTeam1 = true;
            }

            foreach (var g in x)
            {
                games.Add(g);
            }

            return games.OrderBy(p => p.GameNumber).ToList();
        }
        
        internal void CreateGames(bool HasTwoPause = false)
        {
            /*
             *  Auf dieser Seite findet man Informationen bzgl der Berchnung eines Spielplans
             *  http://www-i1.informatik.rwth-aachen.de/~algorithmus/algo36.php
             * 
             */

            int iBahnCor = 0;               //Korrektur-Wert für Bahn

            //Bei ungerade Zahl an Teams ein virtuelles Team hinzufügen
            if (Teams.Count % 2 == 1)
            {
                Teams.AddVirtualTeam();
            }
            else
            {
                //Gerade Anzahl an Mannschaften
                //Entweder kein Aussetzer oder ZWEI Aussetzer
                if (HasTwoPause)
                {
                    Teams.AddVirtualTeam();
                    Teams.AddVirtualTeam();
                }
            }


            //Über Schleifen die Spiele erstellen, Teams, Bahnen und Anspiel festlegen
            for (int i = 1; i < Teams.Count; i++)
            {
                Game game = new Game
                {
                    Team1 = Teams.GetByStartnummer(Teams.Count),
                    Team2 = Teams.GetByStartnummer(i),
                    GameNumber = Teams.Count - i
                };

                #region Bahn Berechnen

                if (game.IsPauseGame)
                {
                    game.NumberOfArea = 0;
                }
                else
                {
                    if (i <= Teams.Count / 2)
                    {
                        game.NumberOfArea = (Teams.Count / 2) - i + 1;
                    }
                    else
                    {
                        game.NumberOfArea = i - (Teams.Count / 2) + 1;
                    }
                    iBahnCor = game.NumberOfArea;
                }

                #endregion

                #region Anspiel festlegen

                if (i % 2 == 0)
                {
                    game.StartOfPlayTeam1 = false;
                }
                else
                {
                    game.StartOfPlayTeam1 = true;
                }

                #endregion

                Games.Add(game);

                for (int k = 1; k <= (Teams.Count / 2 - 1); k++)
                {
                    game = new Game
                    {
                        GameNumber = Teams.Count - i
                    };

                    #region Team1 festlegen

                    if ((i + k) % (Teams.Count - 1) == 0)
                    {
                        game.Team1 = Teams.GetByStartnummer(Teams.Count - 1);
                    }
                    else
                    {
                        var nrTb = (i + k) % (Teams.Count - 1);
                        if (nrTb < 0)
                        {
                            nrTb = (Teams.Count - 1) + nrTb;
                        }
                        game.Team1 = Teams.GetByStartnummer(nrTb);
                    }

                    #endregion

                    #region Team2 festlegen

                    if ((i - k) % (Teams.Count - 1) == 0)
                    {
                        game.Team2 = Teams.GetByStartnummer(Teams.Count - 1);
                    }
                    else
                    {
                        var nrTa = (i - k) % (Teams.Count - 1);
                        if (nrTa < 0)
                        {
                            nrTa = Teams.Count - 1 + nrTa;
                        }
                        game.Team2 = Teams.GetByStartnummer(nrTa);
                    }

                    #endregion

                    #region Bahn berechnen

                    if (game.IsPauseGame)
                    {
                        game.NumberOfArea = 0;
                    }
                    else
                    {
                        if (iBahnCor != k)
                        {
                            game.NumberOfArea = k;
                        }
                        else
                        {
                            game.NumberOfArea = Teams.Count / 2;
                        }
                    }

                    #endregion

                    #region Anspiel berechnen

                    if (k % 2 == 0)
                    {
                        game.StartOfPlayTeam1 = false;
                    }
                    else
                    {
                        game.StartOfPlayTeam1 = true;
                    }

                    #endregion

                    Games.Add(game);
                }
            }

        }

        internal void MultiplicateGames(int count, bool ChangeStartOfPlay = false)
        {
            var originalGamesList = new Game[Games.Count];
            this.Games.CopyTo(originalGamesList);

            for (int i = 1; i < count; i++)
            {
                foreach (var game in originalGamesList)
                {
                    if (i % 2 == 0 && ChangeStartOfPlay)
                    {
                        if (game.StartOfPlayTeam1)
                            game.StartOfPlayTeam1 = false;
                        else
                            game.StartOfPlayTeam1 = true;
                    }
                    Games.Add(game);
                }
            }
        }

        #region CodeFromOldSourceCode
        //--------------
        /// <summary>
        /// Es wird ein Spielplan generiert, in dem zwei Vereine gegeneinander spielen, ohne dass ein Verein ein vereinsinternes Spiel hat
        /// </summary>
        /// <param name="AnzahlTemaS"></param>
        /// <param name="AnzahlAussetzeR"></param>
        /// <param name="Anzahlrunden"></param>
        /// <param name="AspielWechsel"></param>
        /// <returns></returns>
        //public static SPIEL[] Generiere_SonderSpielPlan_1(int AnzahlTeamS, int AnzahlAussetzeR, int AnzahlRunden, bool AnspielWechsel)
        //{
        //    List<SPIEL> Matches = new List<SPIEL>();
        //    SPIEL sMatch;

        //    // Der erste Verein hat gerade Startnummern (2,4,6,8,10) der andere Verein die ungeraden (1-3-5-7-9)
        //    // Immer ohne Aussetzer
        //    // Anzahl der Spiele ist immer Anzahl der Teams / 2
        //    // Anzahl der Bahnen ist immer Anzhal der Teams / 2

        //    int m_AnzahlBahnen = AnzahlTeamS / 2;
        //    int m_AnzahlSpiele = AnzahlTeamS / 2;

        //    int m_BahnMerker;
        //    int m_SpielMerker;

        //    for (int r = 0; r < AnzahlRunden; r++)
        //    {
        //        //Mit Spiel 1 auf Bahn 1 wird begonnen --> es folgen alle ungeraden, da immer +2 gerechnet wird
        //        m_BahnMerker = 1;
        //        m_SpielMerker = 1;

        //        for (int x = 0; x < AnzahlTeamS / 2; x++)
        //        {

        //            for (int y = 0; y < AnzahlTeamS / 2; y++)
        //            {
        //                sMatch = new SPIEL();


        //                sMatch.Runde = r + 1;

        //                if (r % 2 == 0)
        //                {
        //                    sMatch.TeamA = 1 + (y * 2);

        //                    sMatch.TeamB = sMatch.TeamA + 1 + ((x) * 2);
        //                    if (sMatch.TeamB > AnzahlTeamS) { sMatch.TeamB = sMatch.TeamB - AnzahlTeamS; }
        //                }
        //                else
        //                {
        //                    sMatch.TeamB = 1 + (y * 2);

        //                    sMatch.TeamA = sMatch.TeamB + 1 + ((x) * 2);
        //                    if (sMatch.TeamA > AnzahlTeamS) { sMatch.TeamA = sMatch.TeamA - AnzahlTeamS; }
        //                }



        //                sMatch.Bahn = m_BahnMerker + y;
        //                if (sMatch.Bahn > m_AnzahlBahnen)
        //                {
        //                    int tempBahn = sMatch.Bahn;
        //                    sMatch.Bahn = (tempBahn - m_AnzahlBahnen);
        //                }

        //                sMatch.Spiel = m_SpielMerker;
        //                if (sMatch.Spiel > m_AnzahlSpiele)
        //                {
        //                    int tempSpiel = sMatch.Spiel;
        //                    sMatch.Spiel = (tempSpiel - m_AnzahlSpiele);
        //                }


        //                if (x % 2 != 0)
        //                {
        //                    sMatch.Anspiel = sMatch.TeamA;
        //                }
        //                else
        //                {
        //                    sMatch.Anspiel = sMatch.TeamB;
        //                }


        //                Matches.Add(sMatch);

        //            }

        //            m_BahnMerker += 2;
        //            m_SpielMerker += 2;

        //            //Es wird alles auf 2 gesetzt, es folgen alle geraden, da immer +2 gerechnet wird
        //            if (m_BahnMerker > m_AnzahlBahnen) { m_BahnMerker = 2; }
        //            if (m_SpielMerker > m_AnzahlSpiele) { m_SpielMerker = 2; }

        //        }
        //    }
        //    return Matches.ToArray();
        //}
        //-----
        #endregion
    }
}
