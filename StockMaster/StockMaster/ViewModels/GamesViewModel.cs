using StockMaster.BaseClasses;
using StockMaster.Commands;
using System;
using System.Collections.Generic;
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
                if (tournament.NumberOfCourts == value) return;

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
                if (tournament.NumberOfPauseGames == value) return;

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
                if (tournament.NumberOfGameRounds == value) return;

                tournament.NumberOfGameRounds = value;
                RaisePropertyChanged();
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
                        CreateGamesAction();
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
                        Parallel.ForEach(tournament.Teams, (t) =>
                        {
                            t.Games.Clear();
                        });
                    }
                    ));
            }
        }


        private void CreateGamesAction()
        {
            throw new NotImplementedException();
        }
    }

    public class GamesDesignViewModel : IGamesViewModel
    {
        public GamesDesignViewModel()
        {

        }

        public int NumberOfCourts { get; set; } = 4;
        public int NumberOfPauseGames { get; set; } = 1;
        public int NumberOfGameRounds { get; set; } = 1;
    }
}
