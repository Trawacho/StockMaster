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
    public class Turn : TBaseClass
    {
        private int pointsTeamA;
        private int pointsTeamB;
        private int number;

        /// <summary>
        /// Stockpunkte von Team A in dieser Kehre
        /// </summary>
        public int PointsTeamA
        {
            get => pointsTeamA;
            set
            {
                if (pointsTeamA == value)
                    return;

                pointsTeamA = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Stockpunkte von Team B in dieser Kehre
        /// </summary>
        public int PointsTeamB
        {
            get => pointsTeamB;
            set
            {
                if (pointsTeamB == value)
                    return;

                pointsTeamB = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Nummer der Kehre (1 - normalerweise 6)
        /// </summary>
        public int Number
        {
            get => number;
            set
            {
                if (number == value)
                    return;

                number = value;
                RaisePropertyChanged();
            }
        }

        public Turn(int TurnNumber)
        {
            Number = TurnNumber;
            PointsTeamA = 0;
            PointsTeamB = 0;
        }
    }
}
