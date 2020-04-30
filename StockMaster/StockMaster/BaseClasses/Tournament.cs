using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Tournament : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementiation

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Liste aller Teams
        /// </summary>
        public Teams Teams { get; set; }

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
        public int CountOfCourts { get; set; }

        #endregion

        #region Constructor

        public Tournament()
        {
            this.Teams = new Teams();
            //this.Games = new List<Game>();
        }

        #endregion

        #region Functions

        public IEnumerable<Game> GetAllGames()
        {
            return Teams.SelectMany(g => g.Games);
        }

        public void DeleteAllTurnsInEveryGame()
        {
            Parallel.ForEach(GetAllGames(), (g) =>
            {
                g.Turns.Clear();
            });
        }

        public IEnumerable<Game> GetGamesOfCourt(int courtNumber)
        {
            return Teams.SelectMany(g => g.Games)
                .Distinct()
                .Where(c => c.CourtNumber == courtNumber)
                .OrderBy(r => r.RoundOfGame)
                .ThenBy(s => s.GameNumber);
        }

        public IEnumerable<Game> GetGamesOfTeam(int startNumber)
        {
            return Teams.First(t => t.StartNumber == startNumber)?.Games;
        }


        public IEnumerable<Team> GetTeamsRanked()
        {
                return Teams
                        .Where(v => !v.IsVirtual)
                        .OrderByDescending(t => t.SpielPunkte.positiv)
                        .ThenByDescending(p => p.StockNote)
                        .ThenByDescending(d => d.StockPunkteDifferenz);
        }

        #endregion

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
                    TeamB = Teams.GetByStartnummer(Teams.Count),
                    TeamA = Teams.GetByStartnummer(i),
                    GameNumber = Teams.Count - i
                };

                #region Bahn Berechnen

                if (game.IsPauseGame)
                {
                    game.CourtNumber = 0;
                }
                else
                {
                    if (i <= Teams.Count / 2)
                    {
                        game.CourtNumber = (Teams.Count / 2) - i + 1;
                    }
                    else
                    {
                        game.CourtNumber = i - (Teams.Count / 2) + 1;
                    }
                    iBahnCor = game.CourtNumber;
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

                game.TeamA.Games.Add(game);
                game.TeamB.Games.Add(game);
                //Games.Add(game);

                for (int k = 1; k <= (Teams.Count / 2 - 1); k++)
                {
                    game = new Game
                    {
                        GameNumber = Teams.Count - i
                    };

                    #region Team1 festlegen

                    if ((i + k) % (Teams.Count - 1) == 0)
                    {
                        game.TeamB = Teams.GetByStartnummer(Teams.Count - 1);
                    }
                    else
                    {
                        var nrTb = (i + k) % (Teams.Count - 1);
                        if (nrTb < 0)
                        {
                            nrTb = (Teams.Count - 1) + nrTb;
                        }
                        game.TeamB = Teams.GetByStartnummer(nrTb);
                    }

                    #endregion

                    #region Team2 festlegen

                    if ((i - k) % (Teams.Count - 1) == 0)
                    {
                        game.TeamA = Teams.GetByStartnummer(Teams.Count - 1);
                    }
                    else
                    {
                        var nrTa = (i - k) % (Teams.Count - 1);
                        if (nrTa < 0)
                        {
                            nrTa = Teams.Count - 1 + nrTa;
                        }
                        game.TeamA = Teams.GetByStartnummer(nrTa);
                    }

                    #endregion

                    #region Bahn berechnen

                    if (game.IsPauseGame)
                    {
                        game.CourtNumber = 0;
                    }
                    else
                    {
                        if (iBahnCor != k)
                        {
                            game.CourtNumber = k;
                        }
                        else
                        {
                            game.CourtNumber = Teams.Count / 2;
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

                    game.TeamA.Games.Add(game);
                    game.TeamB.Games.Add(game);

                    //Games.Add(game);

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
