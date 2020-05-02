using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;
using System;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDialogService dialogService;

        private NetworkService _NetworkService;
        private Tournament tournament;

        public BaseViewModel ViewModel { get; set; }


        public MainViewModel()
        {
            CreateNewTournament();
            ViewModel = new TournamentViewModel(tournament);
        }
        public MainViewModel(IDialogService dialogService) : this()
        {
            this.dialogService = dialogService;
        }

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
                                            //RaisePropertyChanged(nameof(Ergebnisliste));
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
                    }
                    ));
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


        public Action ExitApplicationAction { get; set; }


        public string UdpButtonContent
        {
            get
            {
                if (_NetworkService == null)
                    return "Start";

                return _NetworkService.IsRunning() ? "Stop" : "Start";
            }
        }



        private void CreateNewTournament()
        {
            tournament = new Tournament
            {
                NumberOfCourts = 4 // 4 Bahnen
            };
            tournament.Teams.Add(new BaseClasses.Team("ESF Hankofen"));
            tournament.Teams.Add(new BaseClasses.Team("EC Pilsting"));
            tournament.Teams.Add(new BaseClasses.Team("DJK Leiblfing"));
            tournament.Teams.Add(new BaseClasses.Team("ETSV Hainsbach"));
            tournament.Teams.Add(new BaseClasses.Team("SV Salching"));
            tournament.Teams.Add(new BaseClasses.Team("SV Haibach"));
            tournament.Teams.Add(new BaseClasses.Team("TSV Bogen"));
            tournament.Teams.Add(new BaseClasses.Team("EC EBRA Aiterhofen"));
            tournament.Teams.Add(new BaseClasses.Team("EC Welchenberg"));

            tournament.CreateGames(true);

        }


    }
}
