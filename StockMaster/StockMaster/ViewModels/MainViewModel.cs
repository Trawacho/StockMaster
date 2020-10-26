using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Fields

        private readonly IDialogService dialogService;

        private readonly NetworkService _NetworkService;
        private Tournament _Tournament;
        private BaseViewModel _viewModel;

        private string tournamentFileName = string.Empty;

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

        public string VersionNumber
        {
            get
            {
                return $"Version: {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructror for DesignView and as BaseConstructor
        /// </summary>
        public MainViewModel()
        {
            this._Tournament = new Tournament();
            ViewModel = new TournamentViewModel(_Tournament);
            this._NetworkService = new NetworkService(this._Tournament, () => this._Tournament.RaisePropertyChanged(""));
            this._NetworkService.StartStopStateChanged += _NetworkService_StartStopStateChanged;
        }

        private void _NetworkService_StartStopStateChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(UdpButtonContent));
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
                              new LiveResultViewModel(_Tournament, _NetworkService));
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
                                _NetworkService.SwitchStartStopState();
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
                        ViewModel = new TournamentViewModel(_Tournament);
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
                        this.ViewModel = new TeamsViewModel(_Tournament);
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
                        this.ViewModel = new GamesViewModel(_Tournament);
                    },
                    (p) =>
                    {
                        return _Tournament.Teams.Count > 0;
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
                        this.ViewModel = new ResultsViewModel(_Tournament);
                    },
                    (p) =>
                    {
                        return _Tournament.CountOfGames() > 0;
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
                        this._Tournament = new Tournament();
                        ViewModel = new TournamentViewModel(this._Tournament);
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
                        Save(tournamentFileName);
                    });
            }
        }

        private ICommand _SaveAsTournamentCommand;
        public ICommand SaveAsTournamentCommand
        {
            get
            {
                return _SaveAsTournamentCommand ??= new RelayCommand(
                    (p) =>
                    {
                        Save(null);
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
                        try
                        {
                            var ofd = new OpenFileDialog
                            {
                                Filter = "StockMaster Files (*.skmr)|*.skmr",
                                DefaultExt = "skmr"
                            };

                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                var filePath = ofd.FileName;

                                this._Tournament = TournamentExtension.Load(filePath);
                                ViewModel = new TournamentViewModel(this._Tournament);
                                this.tournamentFileName = filePath;
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Fehler beim Öffnen:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    });
            }
        }

        #endregion

        private void Save(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                var saveFileDlg = new SaveFileDialog
                {
                    DefaultExt = "skmr",
                    Filter = "StockMaster File (*skmr)|*.skmr"
                };
                var dlgResult = saveFileDlg.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    fileName = saveFileDlg.FileName;
                }
            }

            try
            {
                TournamentExtension.Save(_Tournament, fileName);
                this.tournamentFileName = fileName;
            }
            catch( Exception ex)
            {
                MessageBox.Show("Fehler beim Speicher:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
