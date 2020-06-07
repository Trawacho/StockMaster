using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;
using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Fields

        private readonly IDialogService dialogService;

        private NetworkService _NetworkService;
        private Tournament tournament;
        private LiveResultViewModel liveResultViewModel;
        private BaseViewModel _viewModel;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the ViewModel for the UserControl in the mid of the page
        /// </summary>
        public BaseViewModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                if (_viewModel == value) return;
                _viewModel = value;
                RaisePropertyChanged();
            }
        }

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
            //this.tournament = TournamentExtension.CreateNewTournament(true);
            this.tournament = new Tournament();
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
                return _showLiveResultCommand ??= new RelayCommand(
                    (p) =>
                    {
                        dialogService.SetOwner(App.Current.MainWindow);
                        dialogService.Show(
                            liveResultViewModel ??= new LiveResultViewModel(tournament));
                    },
                    (p) => true
                    );
            }
        }

        private ICommand _StartStopUdpReceiverCommand;
        public ICommand StartStopUdpReceiverCommand
        {
            get
            {
                return _StartStopUdpReceiverCommand ??=
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
                            );
            }
        }

        private ICommand _showTournamentViewCommand;
        public ICommand ShowTournamentViewCommand
        {
            get
            {
                return _showTournamentViewCommand ??= new RelayCommand(
                    (p) =>
                    {
                        ViewModel = new TournamentViewModel(tournament);
                    },
                    (p) => true
                    );
            }
        }

        private ICommand _showTeamsViewCommand;
        public ICommand ShowTeamsViewCommand
        {
            get
            {
                return _showTeamsViewCommand ??= new RelayCommand(
                    (p) =>
                    {
                        this.ViewModel = new TeamsViewModel(tournament);
                    },
                    (p) => true
                    ); ;
            }
        }

        private ICommand _showGamesViewCommand;
        public ICommand ShowGamesViewCommand
        {
            get
            {
                return _showGamesViewCommand ??= new RelayCommand(
                    (p) =>
                    {
                        this.ViewModel = new GamesViewModel(tournament);
                    },
                    (p) =>
                    {
                        return tournament.Teams.Count > 0;
                    });
            }
        }

        private ICommand _showResultsViewCommand;
        public ICommand ShowResultsViewCommand
        {
            get
            {
                return _showResultsViewCommand ??= new RelayCommand(
                    (p) =>
                    {
                        this.ViewModel = new ResultsViewModel(tournament);
                    },
                    (p) =>
                    {
                        return tournament.CountOfGames() > 0;
                    });
            }
        }

        private ICommand _exitApplicationCommand;
        public ICommand ExitApplicationCommand
        {
            get
            {
                return _exitApplicationCommand ??= new RelayCommand(
                    (p) =>
                    {
                        ExitApplicationAction();
                    });
            }
        }

        private ICommand _newTournamentCommand;
        public ICommand NewTournamentCommand
        {
            get
            {
                return _newTournamentCommand ??= new RelayCommand(
                    (p) =>
                    {
                        this.tournament = new Tournament();
                        ViewModel = new TournamentViewModel(this.tournament);
                    });
            }
        }

        private ICommand _SaveTournamentCommand;
        public ICommand SaveTournamentCommand
        {
            get
            {
                return _SaveTournamentCommand ??= new RelayCommand(
                    (p) =>
                    {
                        var sfd = new SaveFileDialog
                        {
                            DefaultExt = "skmr",
                            Filter = "StockMaster File (*.skmr)|*.skmr"
                        };
                        var dialogResult = sfd.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            var filePath = sfd.FileName;
                            TournamentExtension.Save(tournament, filePath);
                        }
                    });
            }
        }

        private ICommand _OpenTournamentCommand;
        public ICommand OpenTournamentCommand
        {
            get
            {
                return _OpenTournamentCommand ??= new RelayCommand(
                    (p) =>
                    {
                        var ofd = new OpenFileDialog
                        {
                            Filter = "StockMaster Files (*.skmr)|*.skmr",
                            DefaultExt = "skmr"
                        };

                        var dialogResult = ofd.ShowDialog();

                        if (dialogResult == DialogResult.OK)
                        {
                            var filePath = ofd.FileName;

                            this.tournament = TournamentExtension.Load(filePath);
                            ViewModel = new TournamentViewModel(this.tournament);
                        }


                    });
            }
        }

        #endregion


    }
}
