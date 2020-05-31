using StockMaster.BaseClasses;
using System;

namespace StockMaster.ViewModels
{
    public interface ITournamentViewModel
    {
        string Venue { get; set; }
        string Organizer { get; set; }
        string Operator { get; set; }
        string TournamentName { get; set; }
        DateTime DateOfTournament { get; set; }

        EntryFee EntryFee { get; set; }

        bool IsDirectionOfCourtsFromRightToLeft { get; set; }
        string DirectionOfCourtsDescription { get; }
        int NumberOfPlayerPerTeam { get; set; }

        Referee Schiedsrichter { get; set; }
        CompetitionManager Wettbewerbsleiter { get; set; }
        ComputingOfficer Rechenbüro{ get; set; }
    }

    public class TournamentViewModel : BaseViewModel, ITournamentViewModel
    {

        private readonly Tournament Tournament;

        public TournamentViewModel(Tournament tournament)
        {
            this.Tournament = tournament;
        }


        /// <summary>
        /// Veranstaltungsort
        /// </summary>
        public string Venue
        {
            get { return Tournament.Venue; }
            set
            {
                if (Tournament.Venue == value)
                    return;

                Tournament.Venue = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Veranstalter
        /// </summary>
        public string Organizer
        {
            get
            {
                return Tournament.Organizer;
            }
            set
            {
                if (Tournament.Organizer == value)
                    return;

                Tournament.Organizer = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Durchführer
        /// </summary>
        public string Operator
        {
            get
            {
                return Tournament.Organizer;
            }
            set
            {
                if (Tournament.Organizer == value)
                    return;
                Tournament.Organizer = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Name des Turniers
        /// </summary>
        public string TournamentName
        {
            get
            {
                return Tournament.TournamentName;
            }
            set
            {
                if (Tournament.TournamentName == value)
                    return;

                Tournament.TournamentName = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Zeitpunkt des Turniers
        /// </summary>
        public DateTime DateOfTournament
        {
            get
            {
                return Tournament.DateOfTournament;
            }
            set
            {
                if (Tournament.DateOfTournament == value)
                    return;
                Tournament.DateOfTournament = value;
                RaisePropertyChanged();
            }
        }

        public EntryFee EntryFee
        {
            get
            {
                return Tournament.EntryFee;
            }
            set
            {
                Tournament.EntryFee = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDirectionOfCourtsFromRightToLeft
        {
            get
            {
                return Tournament.IsDirectionOfCourtsFromRightToLeft;
            }
            set
            {
                if (Tournament.IsDirectionOfCourtsFromRightToLeft == value) return;

                Tournament.IsDirectionOfCourtsFromRightToLeft = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DirectionOfCourtsDescription));
            }
        }

        public string DirectionOfCourtsDescription
        {
            get
            {
                return IsDirectionOfCourtsFromRightToLeft
                    ? "1. Bahn rechts, weitere folgen links"
                    : "1. Bahn links, weitere folgen rechts";
            }
        }

        public int NumberOfPlayerPerTeam
        {
            get
            {
                return Tournament.NumberOfPlayersPerTeam;
            }
            set
            {
                Tournament.NumberOfPlayersPerTeam = value;
                RaisePropertyChanged();
            }
        }

        public Referee Schiedsrichter
        {
            get
            {
                return Tournament.Schiedsrichter;
            }
            set
            {
                Tournament.Schiedsrichter = value;
                RaisePropertyChanged();
            }
        }

        public CompetitionManager Wettbewerbsleiter
        {
            get
            {
                return Tournament.Wettbewerbsleiter;
            }
            set
            {
                Tournament.Wettbewerbsleiter = value;
                RaisePropertyChanged();
            }
        }

        public ComputingOfficer Rechenbüro
        {
            get
            {
                return Tournament.Rechenbüro;
            }
            set
            {
                Tournament.Rechenbüro = value;
                RaisePropertyChanged();
            }
        }
    }

    public class TournamentDesignViewModel : ITournamentViewModel
    {
        public TournamentDesignViewModel()
        {
            EntryFee = new EntryFee(30.00, "dreißig");
        }

        public string Venue { get; set; } = "Hankofen";
        public string Organizer { get; set; } = "Kreis 105 Gäuboden-Vorwald";
        public string Operator { get; set; } = "ESF Hankofen";

        public string TournamentName { get; set; } = "Kreispokal Herren auf Asphalt im Sommer";

        public DateTime DateOfTournament { get; set; } = DateTime.Now;

        public bool IsDirectionOfCourtsFromRightToLeft { get; set; } = true;
        public string DirectionOfCourtsDescription
        {
            get
            {
                return IsDirectionOfCourtsFromRightToLeft
                     ? "1. Bahn rechts, weitere folgen links"
                     : "1. Bahn links, weitere folgen rechts";
            }
        }

        public EntryFee EntryFee { get; set; }

        public int NumberOfPlayerPerTeam { get; set; } = 4;

        public Referee Schiedsrichter { get; set; }
        public ComputingOfficer Rechenbüro { get; set; }
        public CompetitionManager Wettbewerbsleiter { get; set; }
    }


}
