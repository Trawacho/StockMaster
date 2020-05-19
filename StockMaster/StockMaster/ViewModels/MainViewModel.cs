using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;
using System;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Fields

        private readonly IDialogService dialogService;

        private NetworkService _NetworkService;
        private Tournament tournament;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the ViewModel for the UserControl in the mid of the page
        /// </summary>
        public BaseViewModel ViewModel { get; set; }

        /// <summary>
        /// Action to Close the Application
        /// </summary>
        public Action ExitApplicationAction { get; set; }


        /// <summary>
        /// Content for the ListenerButton with state related content
        /// </summary>
        public string UdpButtonContent
        {
            get
            {
                if (_NetworkService == null)
                    return "Start Listener";

                return _NetworkService.IsRunning()
                            ? "Stop Listener"
                            : "Start Listener";
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructror for DesignView and as BaseConstructor
        /// </summary>
        public MainViewModel()
        {
            this.tournament = TournamentExtension.CreateNewTournament(true);
            //this.tournament = new Tournament();
            ViewModel = new TournamentViewModel(tournament);
        }
        /// <summary>
        /// Default-Constructor
        /// </summary>
        /// <param name="dialogService"></param>
        public MainViewModel(IDialogService dialogService) : this()
        {
            this.dialogService = dialogService;
        }

        #endregion

        #region Commands

        private ICommand _showLiveResultCommand;
        public ICommand ShowLiveResultCommand
        {
            get
            {
                return _showLiveResultCommand ?? (_showLiveResultCommand = new RelayCommand(
                    (p) =>
                    {
                        var vm = new LiveResultViewModel(tournament);
                        dialogService.Show(vm);
                    },
                    (p) => true
                    ));
            }
        }

        private ICommand _StartStopUdpReceiverCommand;
        public ICommand StartStopUdpReceiverCommand
        {
            get
            {
                return _StartStopUdpReceiverCommand ?? (_StartStopUdpReceiverCommand =
                    new RelayCommand(
                            (p) =>
                            {
                                if (_NetworkService == null)
                                {
                                    _NetworkService = new NetworkService(tournament,
                                        () =>
                                        {
                                            tournament.RaisePropertyChanged("");
                                        });
                                    _NetworkService.Start();
                                }
                                else
                                {
                                    if (_NetworkService.IsRunning())
                                        _NetworkService.Stop();
                                    else
                                        _NetworkService.Start();
                                }
                                RaisePropertyChanged(nameof(UdpButtonContent));
                            },
                            (o) => { return true; }
                            ));
            }
        }

        private ICommand _showTournamentViewCommand;
        public ICommand ShowTournamentViewCommand
        {
            get
            {
                return _showTournamentViewCommand ?? (_showTournamentViewCommand = new RelayCommand(
                    (p) =>
                    {
                        ViewModel = new TournamentViewModel(tournament);
                        RaisePropertyChanged(nameof(ViewModel));
                    },
                    (p) => true
                    ));
            }
        }

        private ICommand _showTeamsViewCommand;
        public ICommand ShowTeamsViewCommand
        {
            get
            {
                return _showTeamsViewCommand ?? (_showTeamsViewCommand = new RelayCommand(
                    (p) =>
                    {
                        this.ViewModel = new TeamsViewModel(tournament);
                        RaisePropertyChanged(nameof(ViewModel));
                    },
                    (p) => true
                    )); ;
            }
        }

        private ICommand _showGamesViewCommand;
        public ICommand ShowGamesViewCommand
        {
            get
            {
                return _showGamesViewCommand ?? (_showGamesViewCommand = new RelayCommand(
                    (p) =>
                    {
                        this.ViewModel = new GamesViewModel(tournament);
                        RaisePropertyChanged(nameof(ViewModel));
                    }
                    ));
            }
        }

        private ICommand _showResultsViewCommand;
        public ICommand ShowResultsViewCommand
        {
            get
            {
                return _showResultsViewCommand ?? (_showResultsViewCommand = new RelayCommand(
                    (p) =>
                    {
                        this.ViewModel = new ResultsViewModel(tournament);
                        RaisePropertyChanged(nameof(ViewModel));
                    }));
            }
        }

        private ICommand _exitApplicationCommand;
        public ICommand ExitApplicationCommand
        {
            get
            {
                return _exitApplicationCommand ?? (_exitApplicationCommand = new RelayCommand(
                    (p) =>
                    {
                        ExitApplicationAction();
                    }));
            }
        }

        #endregion

        
    }
}
