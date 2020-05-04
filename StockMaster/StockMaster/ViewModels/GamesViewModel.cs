using StockMaster.BaseClasses;
using StockMaster.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public interface IGamesViewModel
    {
        int NumberOfCourts { get; set; }
        int NumberOfGameRounds { get; set; }
        int NumberOfPauseGames { get; set; }
        ObservableCollection<Team> Teams { get; }
        ICommand RemoveAllGamesCommand { get; }
        ICommand CreateGamesCommand { get; }
    }

    public class GamesViewModel : BaseViewModel, IGamesViewModel
    {
        private readonly Tournament tournament;

        public GamesViewModel(Tournament tournament)
        {
            this.tournament = tournament;
        }

        /// <summary>
        /// Anzahl der Spielbahnen
        /// </summary>
        public int NumberOfCourts
        {
            get
            {
                return tournament.NumberOfCourts;
            }
            set
            {
                tournament.NumberOfCourts = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Anzahl der Aussetzer pro Mannschaft
        /// </summary>
        public int NumberOfPauseGames
        {
            get
            {
                return tournament.NumberOfPauseGames;
            }
            set
            {
                tournament.NumberOfPauseGames = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Anzahl der Spielrunden
        /// </summary>
        public int NumberOfGameRounds
        {
            get
            {
                return tournament.NumberOfGameRounds;
            }
            set
            {
                tournament.NumberOfGameRounds = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<Team> Teams
        {
            get
            {
                return new ObservableCollection<Team>(tournament.Teams.Where(t => !t.IsVirtual));
            }
        }



        private ICommand _createGamesCommand;
        public ICommand CreateGamesCommand
        {
            get
            {
                return _createGamesCommand ?? (_createGamesCommand = new RelayCommand(
                    (p) =>
                    {
                        Parallel.ForEach(tournament.Teams, (t) => t.ClearGames());
                        tournament.CreateGames();
                        RaisePropertyChanged(nameof(Teams));
                    }
                    ));

            }
        }

        private ICommand _removeAllGamesCommand;
        public ICommand RemoveAllGamesCommand
        {
            get
            {
                return _removeAllGamesCommand ?? (_removeAllGamesCommand = new RelayCommand(
                    (p) =>
                    {
                        Parallel.ForEach(tournament.Teams, (t) => t.ClearGames());
                        tournament.RemoveAllVirtualTeams();
                        RaisePropertyChanged(nameof(Teams));
                    }
                    ));
            }
        }

    }

    public class GamesDesignViewModel : IGamesViewModel
    {
        private readonly Tournament t = new Tournament();


        public GamesDesignViewModel()
        {
            t.CreateGames();
        }

        public int NumberOfCourts { get; set; } = 4;
        public int NumberOfPauseGames { get; set; } = 1;
        public int NumberOfGameRounds { get; set; } = 1;

        public ObservableCollection<Team> Teams { get { return new ObservableCollection<Team>(t.Teams.Where(t => !t.IsVirtual)); } }

        public ICommand RemoveAllGamesCommand { get; }
        public ICommand CreateGamesCommand { get; }

    }
}
