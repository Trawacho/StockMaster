using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Dialogs;

namespace StockMaster.Models
{
    public class MainViewModel : TBaseClass
    {
        private readonly IDialogService dialogService;
        private readonly IWindowService windowService;


        private NetworkService _NetworkService;




        public Tournament tournament;
        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public MainViewModel(IWindowService windowService)
        {
            this.windowService = windowService;

            CreateNewTournament();
        }

        private RelayCommand _showLiveResultCommand;
        public RelayCommand ShowLiveResultCommand
        {
            get
            {
                return _showLiveResultCommand ?? (_showLiveResultCommand = new RelayCommand(
                    (p) => ShowLiveResultAction(),
                    (p) => true
                    ));
            }
        }

        private RelayCommand _StartStopUdpReceiverCommand;
        public RelayCommand StartStopUdpReceiverCommand
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



        public string UdpButtonContent
        {
            get
            {
                if (_NetworkService == null)
                    return "Start";

                return _NetworkService.IsRunning() ? "Stop" : "Start";
            }
        }





        private void ShowLiveResultAction()
        {
            var vm = new LiveResultViewModel(tournament);

            windowService.Show(vm);
        }

        private void CreateNewTournament()
        {
            tournament = new Tournament
            {
                CountOfCourts = 4 // 4 Bahnen
            };
            tournament.Teams.Add(new BaseClasses.Team(1, "ESF Hankofen"));
            tournament.Teams.Add(new BaseClasses.Team(2, "EC Pilsting"));
            tournament.Teams.Add(new BaseClasses.Team(3, "DJK Leiblfing"));
            tournament.Teams.Add(new BaseClasses.Team(4, "ETSV Hainsbach"));
            tournament.Teams.Add(new BaseClasses.Team(5, "SV Salching"));
            tournament.Teams.Add(new BaseClasses.Team(6, "SV Haibach"));
            tournament.Teams.Add(new BaseClasses.Team(7, "TSV Bogen"));
            tournament.Teams.Add(new BaseClasses.Team(8, "EC EBRA Aiterhofen"));
            tournament.Teams.Add(new BaseClasses.Team(9, "EC Welchenberg"));

            tournament.CreateGames(true);


        }




    }
}
