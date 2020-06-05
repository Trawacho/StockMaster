﻿using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Interfaces;
using StockMaster.Output;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public class GamesViewModel : BaseViewModel, IGamesViewModel
    {
        #region Fields

        private readonly Tournament tournament;

        #endregion

        #region Constructor

        public GamesViewModel(Tournament tournament)
        {
            this.tournament = tournament;
            ConcatRoundsOnOutput = false;
            TeamNameOnTurnCards = false;
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

        /// <summary>
        /// Liste der Reellen Teams
        /// </summary>
        public ObservableCollection<Team> Teams
        {
            get
            {
                return new ObservableCollection<Team>(tournament.Teams.Where(t => !t.IsVirtual));
            }
        }

        /// <summary>
        /// bei gerader Anzahl an Manschaften, true, dann werden zwei Aussetzer gespielt
        /// </summary>
        public bool TwoPauseGames
        {
            get { return tournament.TwoPauseGames; }
            set
            {
                tournament.TwoPauseGames = value;
                RaisePropertyChanged();
            }
        }

       

        private bool concatRoundsOnOutput;
        public bool ConcatRoundsOnOutput
        {
            get
            {
                return NumberOfGameRounds == 1 ? true : concatRoundsOnOutput;
            }
            set
            {
                if (concatRoundsOnOutput == value) return;

                concatRoundsOnOutput = value;
                RaisePropertyChanged();
            }
        }


        public bool IsDirectionOfCourtsFromRightToLeft
        {
            get
            {
                return tournament.IsDirectionOfCourtsFromRightToLeft;
            }
            set
            {
                if (tournament.IsDirectionOfCourtsFromRightToLeft == value) return;

                tournament.IsDirectionOfCourtsFromRightToLeft = value;
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


        public bool TeamNameOnTurnCards { get; set; }

        public bool Is8KehrenSpiel
        {
            get
            {
                return tournament.Is8KehrenSpiel;
            }
            set
            {
                tournament.Is8KehrenSpiel = value;
                RaisePropertyChanged();
            }
        }

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
                    },
                    (p) =>
                    {
                        return tournament.CountOfGames() == 0;
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
                    },
                    (p) =>
                    {
                        return tournament.CountOfGames() > 0;
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
                        var x = new Output.Wertungskarte.Wertungskarte();
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

        public int NumberOfCourts { get; }
        public int NumberOfGameRounds { get; set; } = 1;
        public bool TwoPauseGames { get; set; } = true;

        public ObservableCollection<Team> Teams { get { return new ObservableCollection<Team>(t.Teams.Where(t => !t.IsVirtual)); } }

        public bool ConcatRoundsOnOutput { get; set; } = true;
        public bool TeamNameOnTurnCards { get; set; } = true;

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

        public bool Is8KehrenSpiel { get; set; } = true;

        public ICommand RemoveAllGamesCommand { get; }
        public ICommand CreateGamesCommand { get; }
        public ICommand PrintTurnCardsCommand { get; }

    }
}
