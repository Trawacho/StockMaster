using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Game : TBaseClass, IEquatable<Game>
    {
        private int roundOfGame;
        private int gameNumber;
        private int gameNumberOverall;
        private int courtNumber;
        private bool startOfPlayTeam1;

        #region IEquatable Implementation

        public bool Equals(Game other)
        {
            return (this.RoundOfGame == other.RoundOfGame
                 && this.CourtNumber == other.CourtNumber
                 && this.GameNumber == other.GameNumber);
        }
        public override bool Equals(object obj)
        {
            if (obj is Game game)
            {
                return this.Equals(game);
                //return (this.RoundOfGame == game.RoundOfGame
                //      && this.CourtNumber == game.CourtNumber
                //      && this.GameNumber == game.GameNumber);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion


        #region Properties

        /// <summary>
        /// SpielRunde (für DoppelRunde oder mehrfachrunden)
        /// </summary>
        public int RoundOfGame
        {
            get => roundOfGame;
            set
            {
                if (roundOfGame == value)
                    return;
                roundOfGame = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Nummer vom laufenden Spiel
        /// </summary>
        public int GameNumber
        {
            get => gameNumber; set
            {
                if (gameNumber == value)
                    return;

                gameNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Nummer des Spiels über alle Runden hinweg
        /// </summary>
        public int GameNumberOverAll
        {
            get => gameNumberOverall;
            set
            {
                if (gameNumberOverall == value) return;
                gameNumberOverall = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Nummer der Spielbahn
        /// </summary>
        public int CourtNumber
        {
            get => courtNumber;
            set
            {
                if (courtNumber == value)
                    return;

                courtNumber = value;
                RaisePropertyChanged();
            }

        }

        /// <summary>
        /// Team A - 1
        /// </summary>
        public Team TeamA { get; set; }

        /// <summary>
        /// Team B - 2
        /// </summary>
        public Team TeamB { get; set; }

        /// <summary>
        /// Das Team A hat Anspiel
        /// </summary>
        public bool StartOfPlayTeamA
        {
            get => startOfPlayTeam1;
            set
            {
                if (startOfPlayTeam1 == value)
                    return;

                startOfPlayTeam1 = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Das anspielende Team
        /// </summary>
        public Team StartingTeam
        {
            get
            {
                return StartOfPlayTeamA ? TeamA : TeamB;
            }
        }

        /// <summary>
        /// Wer ist der Gegner
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public Team GetOpponent(Team team)
        {
            return team == TeamA ? TeamB : TeamA;
        }

        /// <summary>
        /// Dieses Spiel ist ein Aussetzer, der Gegner ist ein Virtueller
        /// </summary>
        public bool IsPauseGame
        {
            get
            {
                if (TeamA.IsVirtual || TeamB.IsVirtual)
                    return true;
                else
                    return false;
            }
        }
        public bool IsNotPauseGame
        {
            get
            {
                return !IsPauseGame;
            }
        }

        /// <summary>
        /// Liste der Kehren
        /// </summary>
        public ConcurrentStack<Turn> Turns { get; set; }

        /// <summary>
        /// Summe der Stockpunkte von TeamB in diesem Spiel
        /// </summary>
        public int StockPointsTeamB
        {
            get
            {
                return Turns?.Sum(x => x.PointsTeamB) ?? 0;
            }
        }

        /// <summary>
        /// Summe der Stockpunkte von TeamA in diesem Spiel
        /// </summary>
        public int StockPointsTeamA
        {
            get
            {
                return Turns?.Sum(x => x.PointsTeamA) ?? 0;
            }
        }


        /// <summary>
        /// Spielpunkte von TeamB in diesem Spiel
        /// </summary>
        public int SpielPunkteTeamB
        {
            get
            {
                if (Turns.Count == 0)
                    return 0;

                if (StockPointsTeamA > StockPointsTeamB)
                    return 0;

                if (StockPointsTeamB > StockPointsTeamA)
                    return 2;

                return 1;
            }
        }

        /// <summary>
        /// Spielpunkte von TeamA in diesem Spiel
        /// </summary>
        public int SpielPunkteTeamA
        {
            get
            {
                if (Turns.Count == 0)
                    return 0;

                if (StockPointsTeamA > StockPointsTeamB)
                    return 2;

                if (StockPointsTeamB > StockPointsTeamA)
                    return 0;

                return 1;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Game()
        {
            RoundOfGame = 1;
            this.Turns = new ConcurrentStack<Turn>();
        }



        #endregion


        public override string ToString()
        {
            return $"R#:{RoundOfGame} C#:{CourtNumber} G#:{GameNumber}({GameNumberOverAll}) -- {TeamA.StartNumber} : {TeamB.StartNumber}    T1A:{StartOfPlayTeamA}     P:{IsPauseGame} ";
        }


        #region Public Functions

        public int GetStockPunkte(Team team)
        {
            if (team == TeamA)
                return StockPointsTeamA;
            if (team == TeamB)
                return StockPointsTeamB;
            return 0;
        }

        public int GetStockPunkteGegner(Team team)
        {
            if (team == TeamA)
                return StockPointsTeamB;
            if (team == TeamB)
                return StockPointsTeamA;
            return 0;
        }

        public int GetSpielPunkte(Team team)
        {
            if (team == TeamA)
                return SpielPunkteTeamA;
            if (team == TeamB)
                return SpielPunkteTeamB;
            return 0;
        }

        public int GetSpielPunkteGegner(Team team)
        {
            if (team == TeamA)
                return SpielPunkteTeamB;
            if (team == TeamB)
                return SpielPunkteTeamA;
            return 0;
        }

        #endregion
    }
}
