using StockApp.BaseClasses;
using StockApp.BaseClasses.Zielschiessen;
using StockApp.Commands;
using StockApp.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

namespace StockApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Fields

        private readonly IDialogService dialogService;

        private BaseViewModel _viewModel;

        private Turnier _turnier;

        private string tournamentFileName = string.Empty;

        private StockTVs _stockTVs;

        #endregion

        #region Properties

        /// <summary>
        /// Holds the ViewModel for the UserControl in the mid of the page
        /// </summary>
        public BaseViewModel ViewModel
        {
            get => _viewModel;
            set => SetProperty(ref _viewModel, value);

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
                return _turnier.Wettbewerb is TeamBewerb;
            }
        }

        public bool IsZielBewerb
        {
            get
            {
                return _turnier.Wettbewerb is Zielbewerb;
            }
        }

        public string StockTVCount
        {
            get
            {
                return $"StockTV: {_stockTVs?.Count ?? 0}x";
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Parameterless Constructror for DesignView and as BaseConstructor
        /// </summary>
        public MainViewModel()
        {
            this._turnier = new Turnier
            {
                Wettbewerb = new TeamBewerb()
            };

            ViewModel = new TurnierViewModel(_turnier);

            NetworkService.Instance.StartStopStateChanged += NetworkService_StartStopStateChanged;

            this._turnier.PropertyChanged += Turnier_WettbewerbChanged;

            _stockTVs = new StockTVs();
            _stockTVs.StockTVCollectionAdded += StockTVs_StockTVCollectionAdded;
            _stockTVs.StockTVCollectionRemoved += StockTVs_StockTVCollectionRemoved;
            _stockTVs.StartDiscovery();
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

        private void StockTVs_StockTVCollectionRemoved(object sender, StockTVCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"StockTV: {e.StockTV.HostName} with IP [{e.StockTV.IPAddress}] removed");
            RaisePropertyChanged(nameof(StockTVCount));
        }

        private void StockTVs_StockTVCollectionAdded(object sender, StockTVCollectionChangedEventArgs e)
        {
            Debug.WriteLine($"StockTV: {e.StockTV.HostName} with IP [{e.StockTV.IPAddress}] found and added");
            e.StockTV.StockTVResultChanged += StockTV_StockTVResultChanged;
            e.StockTV.StockTVSettingsChanged += StockTV_StockTVSettingsChanged;

            RaisePropertyChanged(nameof(StockTVCount));
        }

        private void StockTV_StockTVSettingsChanged(object sender, StockTVSettingsChangedEventArgs e)
        {
            Debug.WriteLine($"StockTV: [{(sender as StockTV).HostName}] Settings: [{e.TVSettings}] changed");
        }

        private void StockTV_StockTVResultChanged(object sender, StockTVResultChangedEventArgs e)
        {
            Debug.WriteLine($"StockTV: [{(sender as StockTV).HostName}] Result: [{e.TVResult}] changed");
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



        #region Commands


        private ICommand _testCommand;
        public ICommand TestCommand
        {
            get
            {
                return _testCommand ??= new RelayCommand(
                    (p) =>
                    {
                        foreach (StockTV tv in _stockTVs)
                        {
                            // for (int i = 0; i < 2; i++)
                            {
                                tv.TVSettingsGet();
                                tv.TVSettings.GameModus = GameModis.Turnier;
                                tv.TVSettings.PointsPerTurn = 15;
                                tv.TVSettings.TurnsPerGame = 8;
                                tv.TVSettingsSend();
                                tv.TVResultReset();

                                tv.SendTeamNames(new List<StockTVBegegnung>()
                                {
                                    new StockTVBegegnung(1, "ESF Hankofen", "EC EBRA Aiterhofen"),
                                    new StockTVBegegnung(1, "TV Geiselhöring","SV Pilgramsberg"),
                                    new StockTVBegegnung(3, "DJK Leiblfing", "SV Salching"),
                                    new StockTVBegegnung(4, "Bavaria mitterharthausen", "EC Obermiethnach")
                                });
                            }

                        }
                    },
                    (p) => true);
            }
        }



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
                              new LiveResultViewModel(_turnier.Wettbewerb as TeamBewerb));
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
                                    NetworkService.Instance.Start(this._turnier.Wettbewerb);

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
                        ViewModel = new TurnierViewModel(_turnier);
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
                        this.ViewModel = new TeamsViewModel(_turnier);
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
                        this.ViewModel = new GamesViewModel(_turnier.Wettbewerb as TeamBewerb);
                    },
                    (p) =>
                    {
                        return (_turnier.Wettbewerb as TeamBewerb)?.Teams.Count > 0;
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
                        this.ViewModel = new ResultsViewModel(_turnier);
                    },
                    (p) =>
                    {
                        return (_turnier.Wettbewerb as TeamBewerb)?.CountOfGames() > 0;
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
                        this._turnier = new Turnier();
                        ViewModel = new TurnierViewModel(this._turnier);
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
                                this._turnier = TeamBewerbExtension.Load(filePath);
                                ViewModel = new TurnierViewModel(this._turnier);
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
                           this.ViewModel = new ZielSpielerViewModel(_turnier);
                       },
                       (p) =>
                       {
                           return (_turnier.Wettbewerb is Zielbewerb);
                       });
            }
        }


        private ICommand _showStockTVCollectionCommand;
        public ICommand ShowStockTVCollectionCommand
        {
            get
            {
                return
                    _showStockTVCollectionCommand ??= new RelayCommand(
                        (p) =>
                        {
                            this.ViewModel = new StockTVCollectionViewModel(ref _stockTVs);
                        },
                        (p) =>
                        {
                            return true;
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
                    Filter = "StockMaster File (*.skmr)|*.skmr"
                };
                var dlgResult = saveFileDlg.ShowDialog();
                if (dlgResult == DialogResult.OK)
                {
                    fileName = saveFileDlg.FileName;
                }
            }

            try
            {
                TeamBewerbExtension.Save(_turnier, fileName);
                this.tournamentFileName = fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speicher:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void StopNetMq()
        {
            _stockTVs.StopAllServices();
        }




    }
}

