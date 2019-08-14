using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.Models
{
    public class TournamentModel
    {
        public BaseClasses.Tournament Tournament { get; set; }
        public void CreateNewTournament()
        {
            Tournament = new BaseClasses.Tournament();
            Tournament.CountOfAreas = 4; // 4 Bahnen
            Tournament.Teams.Add(new BaseClasses.Team(1, "ESF Hankofen", 4));
            Tournament.Teams.Add(new BaseClasses.Team(2, "EC Pilsting", 4));
            Tournament.Teams.Add(new BaseClasses.Team(3, "DJK Leiblfing", 4));
            Tournament.Teams.Add(new BaseClasses.Team(4, "ETSV Hainsbach", 4));
            Tournament.Teams.Add(new BaseClasses.Team(5, "SV Salching", 4));
            Tournament.Teams.Add(new BaseClasses.Team(6, "SV Haibach", 4));
            Tournament.Teams.Add(new BaseClasses.Team(7, "TSV Bogen", 4));
            Tournament.Teams.Add(new BaseClasses.Team(8, "EC EBRA Aiterhofen", 4));
         //   Tournament.Teams.Add(new BaseClasses.Team(9, "EC Welchenberg", 4));

            Tournament.CreateGames(true);
            foreach (var item in Tournament.Games.OrderBy(x=>x.NumberOfArea).OrderBy(y=>y.GameNumber))
            {
                System.Diagnostics.Debug.Print(
                    $"Spiel: {item.GameNumber}  \r\n" +
                    $"Team1: {item.Team1.StartNumber} - {item.Team1.TeamName}\r\n" +
                    $"Team2: {item.Team2.StartNumber} - {item.Team2.TeamName}\r\n" +
                    $"Anspiel Team1: {item.StartOfPlayTeam1} \r\n" +
                    $"Bahn: {item.NumberOfArea} \r\n");
            }

        }
    }
}
