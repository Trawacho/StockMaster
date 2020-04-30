using StockMaster.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.Models
{
    public class TournamentModel:IDisposable, INotifyPropertyChanged
    {
        static NetworkService ns;

        public event PropertyChangedEventHandler PropertyChanged;
        public  void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public TournamentModel()
        {
            CreateNewTournament();
            this.Tournament.PropertyChanged += Tournament_PropertyChanged;
        }

        private void Tournament_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(this.Ergebnisliste));
        }

        public BaseClasses.Tournament Tournament { get; set; }
        public void CreateNewTournament()
        {
            Tournament = new BaseClasses.Tournament();
            Tournament.CountOfCourts = 4; // 4 Bahnen
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


            foreach (var item in Tournament.Games.OrderBy(x=>x.CourtNumber).OrderBy(y=>y.GameNumber))
            {
                System.Diagnostics.Debug.Print(
                    $"Spiel: {item.GameNumber}  \r\n" +
                    $"Team1: {item.TeamB.StartNumber} - {item.TeamB.TeamName}\r\n" +
                    $"Team2: {item.TeamA.StartNumber} - {item.TeamA.TeamName}\r\n" +
                    $"Anspiel Team1: {item.StartOfPlayTeam1} \r\n" +
                    $"Bahn: {item.CourtNumber} \r\n");
            }
            //BaseClasses.NetworkService.ReceiveBroadcast();
            ns = new NetworkService(Tournament);
            ns.ReceiveBroadcast();
            System.Diagnostics.Debug.WriteLine("....done....and listen...");
        }

        public void Dispose()
        {
            ns.Dispose();
        }

        public ObservableCollection<(int Platzierung, BaseClasses.Team Team)> Ergebnisliste
        {
            get
            {
                var liste = new ObservableCollection<(int _platzierung, BaseClasses.Team _team)>();
                int i = 1;
                foreach (var t in Tournament.Ergebnisliste)
                {
                    liste.Add((i, t));
                    i++;
                }
                return liste;
            }
        }
    }
}
