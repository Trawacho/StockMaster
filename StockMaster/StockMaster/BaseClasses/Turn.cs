using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    /// <summary>
    /// Eine Kehre im Spiel
    /// </summary>
    public class Turn
    {
        /// <summary>
        /// Stockpunkte von Team A in dieser Kehre
        /// </summary>
        public int PointsTeamA { get; set; }

        /// <summary>
        /// Stockpunkte von Team B in dieser Kehre
        /// </summary>
        public int PointsTeamB { get; set; }

        /// <summary>
        /// Nummer der Kehre (1 - normalerweise 6)
        /// </summary>
        public int Number { get; set; }

        public Turn(int TurnNumber)
        {
            Number = TurnNumber;
            PointsTeamA = 0;
            PointsTeamB = 0;
        }
    }
}
