using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Xml.Serialization;

namespace StockMaster.BaseClasses
{
    public class Game : TBaseClass, IEquatable<Game>
    {
        #region Fields

        private int roundOfGame;
        private int gameNumber;
        private int gameNumberOverall;
        private int courtNumber;
        private bool startOfPlayTeam1;
        private Turn masterTurn;
        private int startNumberTeamA;
        private int startNumberTeamB;

        #endregion

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
        [XmlIgnore()]
        public Team TeamA { get; set; }

        /// <summary>
        /// StartNumber of TeamA 
        /// </summary>
        [XmlElement(ElementName = nameof(StartNumberTeamA))]
        public int StartNumberTeamA
        {
            get
            {
                return TeamA?.StartNumber ?? startNumberTeamA;
            }
            [Obsolete("Only used for xml serialization", error: true)]
            set
            {
                if (TeamA != null)
                {
                    throw new NotSupportedException("Setting the StartNumberTeamA property is not supported");
                }

                startNumberTeamA = value;
            }
        }

        /// <summary>
        /// Team B - 2
        /// </summary>
        [XmlIgnore()]
        public Team TeamB { get; set; }

        /// <summary>
        /// StartNumber of TeamB
        /// </summary>
        [XmlElement(ElementName = nameof(StartNumberTeamB))]
        public int StartNumberTeamB
        {
            get
            {
                return TeamB?.StartNumber ?? startNumberTeamB;
            }
            [Obsolete("Only used for xml serialization", error: true)]
            set
            {
                if (TeamB != null)
                    throw new NotSupportedException("Setting the StartNumberTeamB property is not supported");

                startNumberTeamB = value;
            }
        }
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
        /// Das NICHT anspielende Team
        /// </summary>
        public Team NotStartingTeam
        {
            get
            {
                return StartOfPlayTeamA ? TeamB : TeamA;
            }
        }

        /// <summary>
        /// Wer ist der Gegner
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public Team GetOpponent(Team team)
        {
            // return team == TeamA ? TeamB : TeamA;
            return team == TeamA
                         ? TeamB
                         : team == TeamB
                                 ? TeamA
                                 : null;
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
        /// Liste der Kehren von StockTV
        /// </summary>
        [XmlIgnore()]
        public ConcurrentStack<Turn> Turns { get; set; }

        /// <summary>
        /// ErgebnisKehre. Wenn hier Werte enthalten sind, dann wird die Liste der Kehren nicht berücksichtigt
        /// </summary>
        public Turn MasterTurn
        {
            get
            {
                return masterTurn;
            }
            set
            {
                if (masterTurn == value) return;
                masterTurn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Summe der Stockpunkte von TeamB in diesem Spiel
        /// </summary>
        public int StockPointsTeamB
        {
            get
            {
                if (MasterTurn.PointsTeamA + MasterTurn.PointsTeamB > 0)
                {
                    return MasterTurn.PointsTeamB;
                }
                else
                {
                    return Turns?.Sum(x => x.PointsTeamB) ?? 0;
                }
            }
        }

        /// <summary>
        /// Summe der Stockpunkte von TeamA in diesem Spiel
        /// </summary>
        public int StockPointsTeamA
        {
            get
            {
                //In Kehre 0 stehen die manuellen Eingaben. In allen anderen Kehren die Werte von dem NetworkService.
                //Wenn die Werte in Kehre 0 größer 0 sind, werden auch diese genommen und die Werte aus dem NetworkService ignoriert
                if (MasterTurn.PointsTeamA + MasterTurn.PointsTeamB > 0)
                {
                    return MasterTurn.PointsTeamA;
                }
                else
                {
                    return Turns?.Sum(x => x.PointsTeamA) ?? 0;

                }
            }
        }


        /// <summary>
        /// Spielpunkte von TeamB in diesem Spiel
        /// </summary>
        public int SpielPunkteTeamB
        {
            get
            {
                if (StockPointsTeamA == 0 && StockPointsTeamB == 0)
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
                if (StockPointsTeamA == 0 && StockPointsTeamB == 0)
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
            this.MasterTurn = new Turn(0);   //Default-Kehere für manuelle Eingabe
        }



        #endregion

        #region Public Functions

        public override string ToString()
        {
            return $"R#:{RoundOfGame} C#:{CourtNumber} G#:{GameNumber}({GameNumberOverAll}) -- {TeamA.StartNumber} : {TeamB.StartNumber}    T1A:{StartOfPlayTeamA}     P:{IsPauseGame} ";
        }

        /// <summary>
        /// Returns RoundOfGame and GameNumber as a string
        /// </summary>
        public string GameName
        {
            get
            {
                return $"Runde: {RoundOfGame} | Spiel: {GameNumber}";
            }
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



        #endregion
    }
}
