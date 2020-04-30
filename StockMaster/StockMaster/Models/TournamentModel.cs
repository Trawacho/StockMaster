using StockMaster.BaseClasses;
using StockMaster.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StockMaster.Models
{
    public class TournamentModel : TBaseClass, INotifyPropertyChanged
    {
        private NetworkService _NetworkService;

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
                                    _NetworkService = new NetworkService(Tournament, ()=> { RaisePropertyChanged(nameof(Ergebnisliste)); });
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

        public TournamentModel()
        {
            CreateNewTournament();
            
            this.Tournament.PropertyChanged += Tournament_PropertyChanged;
            
            //_NetworkService = new NetworkService(this.Tournament);
        }

        private void Tournament_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(this.Ergebnisliste));
        }

        public Tournament Tournament { get; set; }
        public void CreateNewTournament()
        {
            Tournament = new Tournament
            {
                CountOfCourts = 4 // 4 Bahnen
            };
            Tournament.Teams.Add(new BaseClasses.Team(1, "ESF Hankofen"));
            Tournament.Teams.Add(new BaseClasses.Team(2, "EC Pilsting"));
            Tournament.Teams.Add(new BaseClasses.Team(3, "DJK Leiblfing"));
            Tournament.Teams.Add(new BaseClasses.Team(4, "ETSV Hainsbach"));
            Tournament.Teams.Add(new BaseClasses.Team(5, "SV Salching"));
            Tournament.Teams.Add(new BaseClasses.Team(6, "SV Haibach"));
            Tournament.Teams.Add(new BaseClasses.Team(7, "TSV Bogen"));
            Tournament.Teams.Add(new BaseClasses.Team(8, "EC EBRA Aiterhofen"));
            Tournament.Teams.Add(new BaseClasses.Team(9, "EC Welchenberg"));

            Tournament.CreateGames(true);


        }

        

        public ObservableCollection<(int Platzierung, Team Team)> Ergebnisliste
        {
            get
            {
                var liste = new ObservableCollection<(int _platzierung, Team _team)>();
                int i = 1;
                foreach (var t in Tournament.GetTeamsRanked())
                {
                    liste.Add((i, t));
                    i++;
                }
                return liste;
            }
        }
    }
}
