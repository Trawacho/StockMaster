using System;
using System.Collections.Generic;
using System.Linq;

namespace StockMaster.BaseClasses
{
    public class Team : TBaseClass
    {
        private string teamName;
        private int startNumber;
        private bool isVirtual;

        #region Standard-Properties

        /// <summary>
        /// Liste aller Spieler
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// Teamname
        /// </summary>
        public string TeamName
        {
            get => teamName;
            set
            {
                if (teamName == value)
                    return;

                teamName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Startnummer
        /// </summary>
        public int StartNumber
        {
            get => startNumber;
            set
            {
                if (startNumber == value)
                    return;

                startNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Team wird nur zur Berechnung verwendet
        /// </summary>
        public bool IsVirtual
        {
            get => isVirtual;
            set
            {
                if (isVirtual == value)
                    return;

                isVirtual = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Liste aller Spiele 
        /// </summary>
        public List<Game> Games { get; set; }

        #endregion

        #region ReadOnly Result Properties 

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

                return Math.Round((double)StockPunkte.positiv / StockPunkte.negativ, 3);

            }
        }

        public int StockPunkteDifferenz
        {
            get
            {
                return StockPunkte.positiv - StockPunkte.negativ;
            }
        }

        #endregion 


        #region Constructor

        /// <summary>
        /// Default-Constructor
        /// </summary>
        public Team()
        {
            this.IsVirtual = false;
            this.StartNumber = 0;
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
        /// <param name="TeamName"></param>
        /// <param name="CountOfDefaultPlayer"></param>
        public Team(string TeamName) : this()
        {
            this.TeamName = TeamName;

        }

        /// <summary>
        /// Constructor with Paramters
        /// </summary>
        /// <param name="StartNumber"></param>
        /// <param name="TeamName"></param>
        /// <param name="Players"></param>
        public Team(string TeamName, List<Player> Players) : this(TeamName)
        {
            this.Players = Players;
        }

        #endregion

        public override string ToString()
        {
            return $"{StartNumber}. {TeamName}";
        }

        public Team CopyWithoutGamesOrPlayers()
        {
            var copy = (Team)this.MemberwiseClone();
            Games = new List<Game>();
            Players = new List<Player>();

            return copy;

        }

    }
}
