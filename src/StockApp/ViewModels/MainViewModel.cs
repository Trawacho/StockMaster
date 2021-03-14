using StockApp.BaseClasses;
using StockApp.BaseClasses.Zielschiessen;
using StockApp.Commands;
using StockApp.Dialogs;
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace StockApp.ViewModels
{
    public class MainViewModel : BaseViewModel, IDisposable
    {

        #region Fields

        private readonly IDialogService dialogService;

        private BaseViewModel _viewModel;

        private Turnier _Turnier;

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
                return NetworkService.Instance.IsRunning()
                            ? "Stop Listener"
                            : "Start Listener";
            }
        }

        /// <summary>
        /// Zeigt die Versionsnummer vom Assembly
        /// </summary>
        public string VersionNumber
        {
            get
            {
                return $"Version: {Assembly.GetExecutingAssembly().GetName().Version}";
            }
        }

        public bool IsTeamBewerb
        {
            get
            {
                return _Turnier.Wettbewerb is TeamBewerb;
            }
        }

        public bool IsZielBewerb
        {
            get
            {
                return _Turnier.Wettbewerb is Zielbewerb;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructror for DesignView and as BaseConstructor
        /// </summary>
        public MainViewModel()
        {
            this._Turnier = new Turnier
            {
                Wettbewerb = new TeamBewerb()
            };

            ViewModel = new TurnierViewModel(_Turnier);

            NetworkService.Instance.StartStopStateChanged += NetworkService_StartStopStateChanged;

            this._Turnier.PropertyChanged += Turnier_WettbewerbChanged;
        }

        private void Turnier_WettbewerbChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Turnier.Wettbewerb))
            {
                RaisePropertyChanged(nameof(this.IsTeamBewerb));
                RaisePropertyChanged(nameof(this.IsZielBewerb));
            }
        }

        private void NetworkService_StartStopStateChanged(object sender, EventArgs e)
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
                              new LiveResultViewModel(_Turnier.Wettbewerb as TeamBewerb));
                    },
                    (p) =>
                    {
                        return IsTeamBewerb;
                    }
                    );
            }
        }

        private ICommand _startStopUdpReceiverCommand;
        public ICommand StartStopUdpReceiverCommand
        {
            get
            {
                return _startStopUdpReceiverCommand ??=
                    new RelayCommand(
                            (p) =>
                            {

                                if (NetworkService.Instance.IsRunning())
                                    NetworkService.Instance.Stop();
                                else
                                    NetworkService.Instance.Start(this._Turnier.Wettbewerb);

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
                        ViewModel = new TurnierViewModel(_Turnier);
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
                        this.ViewModel = new TeamsViewModel(_Turnier);
                    },
                    (p) =>
                    {
                        return IsTeamBewerb;
                    }
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
                        this.ViewModel = new GamesViewModel(_Turnier.Wettbewerb as TeamBewerb);
                    },
                    (p) =>
                    {
                        return (_Turnier.Wettbewerb as TeamBewerb)?.Teams.Count > 0;
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
                        this.ViewModel = new ResultsViewModel(_Turnier);
                    },
                    (p) =>
                    {
                        return (_Turnier.Wettbewerb as TeamBewerb)?.CountOfGames() > 0;
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
                        this._Turnier = new Turnier();
                        ViewModel = new TurnierViewModel(this._Turnier);
                    });
            }
        }

        private ICommand _saveTournamentCommand;
        public ICommand SaveTournamentCommand
        {
            get
            {
                return _saveTournamentCommand ??= new RelayCommand(
                    (p) =>
                    {
                        Save(tournamentFileName);
                    });
            }
        }

        private ICommand _saveAsTournamentCommand;
        public ICommand SaveAsTournamentCommand
        {
            get
            {
                return _saveAsTournamentCommand ??= new RelayCommand(
                    (p) =>
                    {
                        Save(null);
                    });
            }
        }

        private ICommand _openTournamentCommand;
        public ICommand OpenTournamentCommand
        {
            get
            {
                return _openTournamentCommand ??= new RelayCommand(
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

                                //this._Tournament = TeamBewerbExtension.Load(filePath);
                                this._Turnier = TeamBewerbExtension.Load(filePath);
                                ViewModel = new TurnierViewModel(this._Turnier);
                                this.tournamentFileName = filePath;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Fehler beim Öffnen:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    });
            }
        }

        private ICommand _showZielSpielerViewCommand;
        public ICommand ShowZielSpielerViewCommand
        {
            get
            {
                return
                   _showZielSpielerViewCommand ??= new RelayCommand(
                       (p) =>
                       {
                           this.ViewModel = new ZielSpielerViewModel(_Turnier);
                       },
                       (p) =>
                       {
                           return (_Turnier.Wettbewerb is Zielbewerb);
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
                TeamBewerbExtension.Save(_Turnier, fileName);
                this.tournamentFileName = fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speicher:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        public void Dispose()
        {
            
        }
    }
}

