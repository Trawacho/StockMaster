using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StockMaster.BaseClasses
{
    public class Tournament : TBaseClass
    {

        #region Properties

        private readonly List<Team> _teams;
        private bool isNumberOfPause2;
        private int numberOfGameRounds;
        private int numberOfPlayersPerTeam;

        /// <summary>
        /// Liste aller Teams
        /// </summary>
        public ReadOnlyCollection<Team> Teams { get; private set; }
        //public List<Team> Teams { get; private set; }


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
        public int NumberOfCourts
        {
            get
            {
                return (Teams.Count(t => !t.IsVirtual) / 2); ;
            }

        }

        /// <summary>
        /// Number of rounds to play (default 1) 
        /// </summary>
        public int NumberOfGameRounds
        {
            get
            {
                return numberOfGameRounds;
            }
            set
            {
                if (numberOfGameRounds == value) return;
                if (value < 1 || value > 3) return;

                numberOfGameRounds = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// True, if Court#1 is on the right. Team with StartNumber 1 is also on right side and goes to left for 2nd game (default True)
        /// </summary>
        public bool IsDirectionOfCourtsFromRightToLeft { get; set; }

        public bool IsNumberOfPause2
        {
            get
            {
                return isNumberOfPause2;
            }
            set
            {
                if (value == isNumberOfPause2) return;
                isNumberOfPause2 = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// On True, the TurnCard has 8 instead of 7 Turns per Team
        /// </summary>
        public bool Is8KehrenSpiel { get; set; }

        /// <summary>
        /// True, wenn bei einer Mehrfachrunde das Anspiel bei jeder Runde gewechselt wird
        /// </summary>
        public bool StartOfTeamChange { get; set; }

        /// <summary>
        /// Startgebühr pro Mannschaft
        /// </summary>
        public EntryFee EntryFee { get; set; }

        /// <summary>
        /// Bei wieviel Mannschaften werden die Spielernamen auf der Ergebnisliste mit angedruckt
        /// </summary>
        public int NumberOfTeamsWithNamedPlayerOnResult { get; set; }

        /// <summary>
        /// Anzahl der Spieler pro Mannschaft (1-6) (default-Wert beim erstellen eines Teams)
        /// </summary>
        public int NumberOfPlayersPerTeam
        {
            get
            {
                return numberOfPlayersPerTeam;
            }
            set
            {
                if (value < Team.MinNumberOfPlayer
                    || value > Team.MaxNumberOfPlayers
                    || value == numberOfPlayersPerTeam)
                    return;

                numberOfPlayersPerTeam = value;
                RaisePropertyChanged();
            }
        }


        public Referee Schiedsrichter { get; set; }
        public CompetitionManager Wettbewerbsleiter { get; set; }
        public ComputingOfficer Rechenbüro { get; set; }

        #endregion

        #region Constructor

        public Tournament()
        {
            this.IsDirectionOfCourtsFromRightToLeft = true;
            this.NumberOfGameRounds = 1;
            this.IsNumberOfPause2 = false;
            this.Is8KehrenSpiel = false;
            this.EntryFee = new EntryFee();
            StartOfTeamChange = false;
            DateOfTournament = DateTime.Now;

            this._teams = new List<Team>();
            this.Teams = _teams.AsReadOnly();
            //this.Teams = _teams;


            this.NumberOfTeamsWithNamedPlayerOnResult = 3;
            this.NumberOfPlayersPerTeam = 4;
            this.Schiedsrichter = new Referee();
            this.Wettbewerbsleiter = new CompetitionManager();
            this.Rechenbüro = new ComputingOfficer();
        }





        #endregion

        #region Functions

        public IEnumerable<Game> GetAllGames()
        {
            return Teams.SelectMany(g => g.Games);
        }

        internal int CountOfGames()
        {
            return GetAllGames().ToList().Count();
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

        public IEnumerable<Team> GetTeamsRanked()
        {
            return Teams
                    .Where(v => !v.IsVirtual)
                    .OrderByDescending(t => t.SpielPunkte.positiv)
                    .ThenByDescending(p => p.StockNote)
                    .ThenByDescending(d => d.StockPunkteDifferenz);
        }




        #endregion

        #region Team Functions

        public void AddVirtualTeams(int count)
        {
            for (int i = 0; i < count; i++)
            {
                AddTeam(new Team("Virtual Team", NumberOfPlayersPerTeam)
                {
                    IsVirtual = true
                },
                i == 0);
            }
        }

        internal void RemoveAllVirtualTeams()
        {
            _teams.RemoveAll(t => t.IsVirtual);
            RaisePropertyChanged(nameof(Teams));
        }

        internal void RemoveAllTeams()
        {
            _teams.Clear();
            RaisePropertyChanged(nameof(Teams));
        }


        /// <summary>
        /// Adds a Team to the Tournament.
        /// - All Games were deleted
        /// - Startnumbers were reOrganized
        /// </summary>
        /// <param name="team">The <see cref="Team"/> to add</param>
        /// <param name="deleteVirtualTeamsFirst">If TRUE, all VirtualTeams are removed first</param>
        public void AddTeam(Team team, bool deleteVirtualTeamsFirst = false)
        {
            if (deleteVirtualTeamsFirst)
                RemoveAllVirtualTeams();

            Parallel.ForEach(_teams, (t) => t.ClearGames());

            ReOrganizeTeamStartNumbers();

            team.StartNumber = _teams.Count + 1;

            this._teams.Add(team);

            RaisePropertyChanged(nameof(Teams));

        }

        /// <summary>
        /// Removes the Team from the Tournament
        /// - Startnumbers were reOrganized
        /// - all Games were deleted
        /// </summary>
        /// <param name="team"></param>
        public void RemoveTeam(Team team)
        {
            this._teams.Remove(team);

            Parallel.ForEach(_teams, (t) => t.ClearGames());

            ReOrganizeTeamStartNumbers();

            RaisePropertyChanged(nameof(Teams));
        }

        /// <summary>
        /// ReOrganize Startnumbers of each Team in List as index-based
        /// </summary>
        private void ReOrganizeTeamStartNumbers()
        {
            for (int i = 0; i < _teams.Count; i++)
            {
                _teams[i].StartNumber = i + 1;
            }
        }

        #endregion

        internal void CreateGames()
        {
            /*
             *  Auf dieser Seite findet man Informationen bzgl der Berchnung eines Spielplans
             *  http://www-i1.informatik.rwth-aachen.de/~algorithmus/algo36.php
             * 
             */
            //Remove Virtual Teams to check the number of VirtualTeams needed
            RemoveAllVirtualTeams();

            int iBahnCor = 0;               //Korrektur-Wert für Bahn

            //Bei ungerade Zahl an Teams ein virtuelles Team hinzufügen
            if (Teams.Count % 2 == 1)
            {
                AddVirtualTeams(1);
            }
            else
            {
                //Gerade Anzahl an Mannschaften
                //Entweder kein Aussetzer oder ZWEI Aussetzer
                //if (NumberOfPauseGames == 2)
                if (IsNumberOfPause2)
                {
                    AddVirtualTeams(2);
                }
            }

            System.Diagnostics.Debug.WriteLine($"Schleifenstart");
            for (int spielRunde = 1; spielRunde <= this.NumberOfGameRounds; spielRunde++)
            {

                //Über Schleifen die Spiele erstellen, Teams, Bahnen und Anspiel festlegen
                for (int i = 1; i < Teams.Count; i++)
                {
                    Game game = new Game
                    {
                        TeamB = Teams.First(t => t.StartNumber == Teams.Count),
                        TeamA = Teams.First(t => t.StartNumber == i),
                        GameNumber = Teams.Count - i,
                        RoundOfGame = spielRunde,
                        GameNumberOverAll = (spielRunde - 1) * (Teams.Count - 1) + (Teams.Count - i)
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

                    game.StartOfPlayTeamA = !(i % 2 == 0);

                    #endregion

                    System.Diagnostics.Debug.WriteLine(game.ToString());

                    game.TeamA.AddGame(game);
                    game.TeamB.AddGame(game);

                    for (int k = 1; k <= (Teams.Count / 2 - 1); k++)
                    {
                        game = new Game
                        {
                            GameNumber = Teams.Count - i,
                            RoundOfGame = spielRunde,
                            GameNumberOverAll = (spielRunde - 1) * (Teams.Count - 1) + (Teams.Count - i)
                        };

                        #region Team A festlegen

                        if ((i + k) % (Teams.Count - 1) == 0)
                        {
                            game.TeamA = Teams.First(t => t.StartNumber == (Teams.Count - 1));
                        }
                        else
                        {
                            var nrTb = (i + k) % (Teams.Count - 1);
                            if (nrTb < 0)
                            {
                                nrTb = (Teams.Count - 1) + nrTb;
                            }
                            game.TeamA = Teams.First(t => t.StartNumber == nrTb);
                        }

                        #endregion

                        #region Team B festlegen

                        if ((i - k) % (Teams.Count - 1) == 0)
                        {
                            game.TeamB = Teams.First(t => t.StartNumber == (Teams.Count - 1));
                        }
                        else
                        {
                            var nrTa = (i - k) % (Teams.Count - 1);
                            if (nrTa < 0)
                            {
                                nrTa = Teams.Count - 1 + nrTa;
                            }
                            game.TeamB = Teams.First(t => t.StartNumber == nrTa);

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

                        game.StartOfPlayTeamA = !(k % 2 == 0);

                        #endregion
                        System.Diagnostics.Debug.WriteLine(game.ToString());

                        game.TeamA.AddGame(game);
                        game.TeamB.AddGame(game);
                    }
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
