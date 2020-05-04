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

        bool IsDirectionOfCourtsFromRightToLeft { get; set; }
        string DirectionOfCourtsDesctiption { get; }
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
                RaisePropertyChanged(nameof(DirectionOfCourtsDesctiption));
            }
        }

        public string DirectionOfCourtsDesctiption
        {
            get
            {
                return IsDirectionOfCourtsFromRightToLeft
                    ? "1. Bahn rechts, weitere folgen links"
                    : "1. Bahn links, weiter folgen rechts";
            }
        }
    }

    public class TournamentDesignViewModel: ITournamentViewModel
    {
        public TournamentDesignViewModel()
        {

        }

        public string Venue { get; set; } = "Hankofen";
        public string Organizer { get; set; } = "Kreis 105 Gäuboden-Vorwald";
        public string Operator { get; set; } = "ESF Hankofen";

        public string TournamentName { get; set; } = "Kreispokal Herren auf Asphalt im Sommer";

        public DateTime DateOfTournament { get; set; } = DateTime.Now;

        public bool IsDirectionOfCourtsFromRightToLeft { get; set; } = true;
        public string DirectionOfCourtsDesctiption
        {
            get
            {
                return IsDirectionOfCourtsFromRightToLeft
                     ? "1. Bahn rechts, weitere folgen links"
                     : "1. Bahn links, weiter folgen rechts";
            }
        }
    }

    
}
