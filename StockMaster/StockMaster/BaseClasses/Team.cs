using System;
using System.Collections.Generic;
using System.Linq;

namespace StockMaster.BaseClasses
{
    public class Team
    {
        #region Standard-Properties
        /// <summary>
        /// Liste aller Spieler
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// Teamname
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Startnummer
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// Team wird nur zur Berechnung verwendet
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// Liste aller Spiele 
        /// </summary>
        public List<Game> Games { get; set; }

        #endregion

       

        public (int positiv, int negativ) SpielPunkte
        {
            get
            {
                return (Games.Sum(g => g.GetSpielPunkte(this)),
                        Games.Sum(g => g.GetSpielPunkteGegner(this)));

            }
        }

        public (int positiv, int negativ) StockPunkte
        {
            get
            {
                return (Games.Sum(g => g.GetStockPunkte(this)),
                        Games.Sum(o => o.GetStockPunkteGegner(this)));
            }
        }

        public double StockNote
        {
            get
            {
                if (StockPunkte.negativ == 0)
                    return StockPunkte.positiv;

                return (double)StockPunkte.positiv / StockPunkte.negativ;

            }
        }

        public int StockPunkteDifferenz
        {
            get
            {
                return StockPunkte.positiv - StockPunkte.negativ;
            }
        }



        #region Constructor

        /// <summary>
        /// Default-Constructor
        /// </summary>
        private Team()
        {
            this.IsVirtual = false;
            this.Players = new List<Player>();
            for (int i = 0; i < 4; i++) // default 4 Spieler erzeugen
            {
                Players.Add(new Player("LastName", "FirstName"));
            }
            this.Games = new List<Game>();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="StartNumber"></param>
        /// <param name="TeamName"></param>
        /// <param name="CountOfDefaultPlayer"></param>
        public Team(int StartNumber, string TeamName) : this()
        {
            this.StartNumber = StartNumber;
            this.TeamName = TeamName;

        }

        /// <summary>
        /// Constructor with Paramters
        /// </summary>
        /// <param name="StartNumber"></param>
        /// <param name="TeamName"></param>
        /// <param name="Players"></param>
        public Team(int StartNumber, string TeamName, List<Player> Players) : this(StartNumber, TeamName)
        {
            this.Players = Players;
        }

        #endregion

        public override string ToString()
        {
            return $"{StartNumber}: { TeamName}";
        }

        public string InfoString
        {
            get
            {
                return String.Format("{0,-3} {1,30} {2,8} {3,5} {4,8}",
                    StartNumber, TeamName, SpielPunkte, StockNote, StockPunkteDifferenz);
                //return String.Format("{0,-10} | {1,-10} | {2,5}", StartNumber, "Gates", 51);
                //return String.Format($"{StartNumber}: {TeamName} \t\t{SpielPunkte.positiv}:{SpielPunkte.negativ}\t{StockNote}\t{StockPunkteDifferenz}");
            }
        }
    }
}
