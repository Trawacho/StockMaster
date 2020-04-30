using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Game : IComparable
    {
        public int CompareTo(object obj)
        {

            if (this.RoundOfGame == ((Game)obj).RoundOfGame &&
                this.CourtNumber == ((Game)obj).CourtNumber &&
                this.GameNumber == ((Game)obj).GameNumber)
            {
                return 0;
            }
            else
            {
                return -1;
            }

        }


        #region Properties

        /// <summary>
        /// SpielRunde (für DoppelRunde oder mehrfachrunden)
        /// </summary>
        public int RoundOfGame { get; set; }


        /// <summary>
        /// Nummer vom laufenden Spiel
        /// </summary>
        public int GameNumber { get; set; }

        /// <summary>
        /// Nummer der Spielbahn
        /// </summary>
        public int CourtNumber { get; set; }

        /// <summary>
        /// Team A - 1 - Rechts
        /// </summary>
        public Team TeamA { get; set; }


        /// <summary>
        /// Team B - 2 Links
        /// </summary>
        public Team TeamB { get; set; }

        /// <summary>
        /// Das Team A hat Anspiel
        /// </summary>
        public bool StartOfPlayTeam1 { get; set; }

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

        object _l = new object();

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
                return Turns.Sum(x => x.PointsTeamB);
            }
        }

        /// <summary>
        /// Summe der Stockpunkte von TeamA in diesem Spiel
        /// </summary>
        public int StockPointsTeamA
        {
            get
            {
                return Turns.Sum(x => x.PointsTeamA);
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

        #endregion


        public Game()
        {
            RoundOfGame = 1;
            this.Turns = new ConcurrentStack<Turn>();
        }

        /// <summary>
        /// Ein Spiel von zwei Teams of einer Bahn
        /// </summary>
        /// <param name="NumberOfArea">Nummer der Bahn</param>
        /// <param name="TeamA">Mannschaft A</param>
        /// <param name="TeamB">Mannschat B</param>
        /// <param name="StartOfPlayTeamA">Hat Team A das Anspiel</param>
        /// <param name="TurnCount">Anzahl der zu spielenden Kehren</param>
        public Game(int NumberOfArea, Team TeamA, Team TeamB, bool StartOfPlayTeamA, int TurnCount = 6) : this()
        {
            if (TeamA == null || TeamB == null)
            {
                throw new ArgumentNullException("nullvalue not allowed for TeamA or TeamB");
            }
            this.CourtNumber = NumberOfArea;
            this.StartOfPlayTeam1 = StartOfPlayTeamA;

            for (int i = 1; i <= TurnCount; i++)
            {
                this.Turns.Push(new Turn(i));
            }
        }


        public override string ToString()
        {
            return $"R#:{RoundOfGame} C#:{CourtNumber} G#:{GameNumber} -- {TeamA.StartNumber}:{TeamB.StartNumber} T1A:{StartOfPlayTeam1}  P:{IsPauseGame} ";
        }

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


    }
}
