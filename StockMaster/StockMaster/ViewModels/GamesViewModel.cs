using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Output;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public interface IGamesViewModel
    {
        int NumberOfCourts { get; set; }
        int NumberOfGameRounds { get; set; }
        int NumberOfPauseGames { get; set; }
        ObservableCollection<Team> Teams { get; }
        int RealTeamsCount { get; }

        bool ConcatRoundsOnOutput { get; set; }
        bool TeamNameOnTurnCards { get; set; }

        ICommand RemoveAllGamesCommand { get; }
        ICommand CreateGamesCommand { get; }

        ICommand PrintTurnCardsCommand { get; }
    }

    public class GamesViewModel : BaseViewModel, IGamesViewModel
    {
        #region Fields

        private readonly Tournament tournament;

        #endregion

        #region Constructor

        public GamesViewModel(Tournament tournament)
        {
            this.tournament = tournament;
            this.PropertyChanged += GamesViewModel_PropertyChanged;
            ConcatRoundsOnOutput = true;
            TeamNameOnTurnCards = false;
        }

        #endregion

        #region Events

        private void GamesViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Teams))
            {
                RaisePropertyChanged(nameof(RealTeamsCount));
            }
        }

        #endregion

        #region Properties


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


        public int RealTeamsCount
        {
            get
            {
                return Teams.Count(t => !t.IsVirtual);
            }
        }

        public bool ConcatRoundsOnOutput { get; set; }
        public bool TeamNameOnTurnCards { get; set; }
        #endregion

        #region Commands


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

        private ICommand _printTurnCardsCommand;
        public ICommand PrintTurnCardsCommand
        {
            get
            {
                return _printTurnCardsCommand ?? (_printTurnCardsCommand = new RelayCommand(
                    (p) =>
                    {
                        var x = new Output.TurnCards.Spiegel();
                        PrintPreview printPreview = new PrintPreview();
                        var A4Size = new System.Windows.Size(8 * 96, 11.5 * 96);
                        printPreview.Document = x.GetDocument(A4Size, tournament, TeamNameOnTurnCards, ConcatRoundsOnOutput);
                        printPreview.ShowDialog();
                    },
                    (p) =>
                    {
                        return tournament.GetAllGames().Count() > 1;
                    }
                    ));
            }
        }

        #endregion

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

        public int RealTeamsCount { get; } = 4;

        public ObservableCollection<Team> Teams { get { return new ObservableCollection<Team>(t.Teams.Where(t => !t.IsVirtual)); } }

        public bool ConcatRoundsOnOutput { get; set; } = true;
        public bool TeamNameOnTurnCards { get; set; } = true;

        public ICommand RemoveAllGamesCommand { get; }
        public ICommand CreateGamesCommand { get; }
        public ICommand PrintTurnCardsCommand { get; }

    }
}
