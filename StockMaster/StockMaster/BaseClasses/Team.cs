using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace StockMaster.BaseClasses
{
    public class Team : TBaseClass, IEquatable<Team>
    {
        #region Fields

        private string teamName;
        private int startNumber;
        private bool isVirtual;
        private string nation;
        private readonly List<Game> games;

        #endregion

        #region IEquatable implementation

        /// <summary>
        /// If a Team is equal to another Team depends only on the StartNumber
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Team other)
        {
            return this.StartNumber == other.StartNumber;
        }

        #endregion

        #region Standard-Properties

        /// <summary>
        /// True, wenn das Team nur zur Berechnung verwendet wird
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
        /// Info über Kreis, Bezirk, Verband oder Nation
        /// </summary>
        public string Nation
        {
            get { return nation; }
            set
            {
                if (value == nation) return;

                nation = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Liste aller Spieler
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// Maximum Number of Players for a Team
        /// </summary>
        public static int MaxNumberOfPlayers { get; } = 6;

        /// <summary>
        /// Minimum Number of Players for a Team
        /// </summary>
        public static int MinNumberOfPlayer { get; } = 0;

        /// <summary>
        /// Liste aller Spiele 
        /// </summary>
        [XmlIgnore()]
        public ReadOnlyCollection<Game> Games { get; private set; }

        /// <summary>
        /// Alle Spielnummern, bei denen die Mannschaft auf einer "steigenden Bahn" ist.
        /// Bedeutet, dass die nächste Bahn eine höhere Nummer hat
        /// </summary>
        [XmlIgnore()]
        public List<int> SteigendeSpielNummern
        {
            get
            {
                List<int> result = new List<int>();
                var sortedGames = Games.OrderBy(g => g.GameNumberOverAll).ToList();
                for (int i = 0; i < sortedGames.Count-1; i++)
                {
                    if (sortedGames[i].CourtNumber <= sortedGames[i + 1].CourtNumber)
                        result.Add(sortedGames[i].GameNumberOverAll);
                }
                return result;
            }
        }


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

        public Team()
        {
            this.IsVirtual = false;
            this.StartNumber = 0;
            this.games = new List<Game>();
            this.Games = games.AsReadOnly();
            this.Players = new List<Player>();
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


        #endregion

        #region Functions

        /// <summary>
        /// Deletes alle Games
        /// </summary>
        public void ClearGames()
        {
            this.games.Clear();
            RaisePropertyChanged(nameof(Games));
        }

        public void AddGame(Game game)
        {
            this.games.Add(game);
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

        internal void RemovePlayer(Player selectedPlayer)
        {
            this.Players.Remove(selectedPlayer);
            RaisePropertyChanged(nameof(Players));
        }

        internal void AddPlayer()
        {
            this.Players.Add(new Player());
            RaisePropertyChanged(nameof(Players));
        }

        public override string ToString()
        {
            return $"{StartNumber}. {TeamName}";
        }

        #endregion

    }
}
