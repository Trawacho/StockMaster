using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StockMaster.BaseClasses
{
    public class Team : TBaseClass, IEquatable<Team>
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

        private readonly List<Game> _games;
        /// <summary>
        /// Liste aller Spiele 
        /// </summary>
        public ReadOnlyCollection<Game> Games { get; private set; }



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

        public string SpielPunkteString
        {
            get
            {
                return $"{SpielPunkte.positiv}:{SpielPunkte.negativ}";
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

        public string StockPunkteString
        {
            get
            {
                return $"{StockPunkte.positiv}:{StockPunkte.negativ}";
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
            this._games = new List<Game>();
            this.Games = _games.AsReadOnly();
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

        /// <summary>
        /// Deletes alle Games
        /// </summary>
        public void ClearGames()
        {
            this._games.Clear();
            RaisePropertyChanged(nameof(Games));
        }

        public void AddGame(Game game)
        {
            this._games.Add(game);
            RaisePropertyChanged(nameof(Games));
        }

        public IOrderedEnumerable<IGrouping<int, Game>> GetGamesGroupedByRound()
        {
            return from game in Games.OrderBy(r => r.RoundOfGame)
                                     .ThenBy(g => g.GameNumber)
                   group game by game.RoundOfGame into grGames
                   orderby grGames.Key
                   select grGames;
        }

        public override string ToString()
        {
            return $"{StartNumber}. {TeamName}";
        }

        [Obsolete]
        public Team CopyWithoutGamesOrPlayers()
        {
            var copy = (Team)this.MemberwiseClone();
            //Games = new List<Game>();
            Players = new List<Player>();

            return copy;

        }

        /// <summary>
        /// If a Team is equal to another Team depends only on the StartNumber
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Team other)
        {
            return this.StartNumber == other.StartNumber;
        }
    }
}
