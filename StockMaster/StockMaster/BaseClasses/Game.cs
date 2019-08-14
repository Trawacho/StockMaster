using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Game
    {
        public int GameNumber { get; set; }

        private Team teamA;

        public Team Team2
        {
            get { return teamA; }
            set { teamA = value; }
        }

        private Team teamB;

        public Team Team1
        {
            get { return teamB; }
            set { teamB = value; }
        }

        private bool startOfPlayTeamA;
        /// <summary>
        /// Das Team A hat Anspiel
        /// </summary>
        public bool StartOfPlayTeam1
        {
            get { return startOfPlayTeamA; }
            set { startOfPlayTeamA = value; }
        }

        private int numberOfArea;
        /// <summary>
        /// Nummer der Spielbahn
        /// </summary>
        public int NumberOfArea
        {
            get { return numberOfArea; }
            set { numberOfArea = value; }
        }

        /// <summary>
        /// Dieses Spiel ist ein Aussetzer, der Gegner ist ein Virtueller
        /// </summary>
        public bool IsPauseGame
        {
            get
            {
                if (Team2.IsVirtual || Team1.IsVirtual)
                    return true;
                else
                    return false;
            }
        }

        public int RoundOfGame { get; set; }


        private List<Turn> turns;
        /// <summary>
        /// Liste der Kehren
        /// </summary>
        public List<Turn> Turns
        {
            get { return turns; }
            internal set { turns = value; }
        }

        /// <summary>
        /// Summe der Stockpunkte von Team A
        /// </summary>
        /// <returns></returns>
        public int GetStockPointsTeamA()
        {
            return turns.Select(x => x.PointsTeamA).Sum();
        }

        /// <summary>
        /// Summe der Stockpunkte von Team B
        /// </summary>
        /// <returns></returns>
        public int GetStockPointsTeamB()
        {
            return turns.Select(x => x.PointsTeamB).Sum();
        }

        /// <summary>
        /// Spielpunkte von Team A (2, 1 oder 0)
        /// </summary>
        /// <returns></returns>
        public int GetPlayPointsTeamA()
        {
            var spa = GetStockPointsTeamA();
            var spb = GetStockPointsTeamB();
            if (spa > spb)
                return 2;
            else if (spa == spb)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Spielpunkte von Team B (2, 1 oder 0)
        /// </summary>
        /// <returns></returns>
        public int GetPlayPointsTeamB()
        {
            var pta = GetStockPointsTeamA();

            if (pta == 0)
                return 2;
            else if (pta == 1)
                return 1;
            else
                return 0;
        }


        public Game()
        {
            RoundOfGame = 1;
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
            this.NumberOfArea = NumberOfArea;
            this.StartOfPlayTeam1 = StartOfPlayTeamA;

            this.Turns = new List<Turn>();
            for (int i = 1; i <= TurnCount; i++)
            {
                this.Turns.Add(new Turn(i));
            }
        }
    }
}
