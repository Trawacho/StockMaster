using System.Collections.Generic;

namespace StockMaster.BaseClasses
{
    public class Team
    {
        private List<Player> players;
        /// <summary>
        /// Liste aller Spieler
        /// </summary>
        public List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        private string teamname;

        /// <summary>
        /// Teamname
        /// </summary>
        public string TeamName
        {
            get { return teamname; }
            set { teamname = value; }
        }

        /// <summary>
        /// Startnummer
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// Team wird nur zur Berechnung verwendet
        /// </summary>
        public bool IsVirtual { get; set; }

        public Team(int StartNumber, string TeamName, int CountOfDefaultPlayer = 4)
        {
            this.IsVirtual = false;
            this.StartNumber = StartNumber;
            this.TeamName = TeamName;
            this.Players = new List<Player>();
            for (int i = 0; i < CountOfDefaultPlayer; i++)
            {
                Players.Add(new Player("LastName", "FirstName"));
            }
        }

        public Team(int StartNumber, string TeamName, List<Player> Players) : this(StartNumber, TeamName)
        {
            this.Players = Players;
        }
    }
}
