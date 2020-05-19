using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    internal class TournamentExtension
    {
        public static Tournament CreateNewTournament(bool generate_9_Teams = false)
        {
            var tournament = new Tournament
            {
                NumberOfCourts = 4, // 4 Bahnen
                NumberOfGameRounds = 1,
                IsNumberOfPause2 = false,
                EntryFee = new EntryFee(30.00, "dreißig"),
                Organizer = "Eisstockfreunde Hankofen",
                DateOfTournament = DateTime.Now,
                Operator = "Kreis 105 Gäuboden/Vorwald",
                TournamentName = "1. Stockturnier Herren 2020",
                Venue = "Hankofen"
            };

            tournament.AddTeam(new Team("ESF Hankofen"));
            tournament.AddTeam(new Team("EC Pilsting"));
            tournament.AddTeam(new Team("DJK Leiblfing"));
            tournament.AddTeam(new Team("ETSV Hainsbach"));
            tournament.AddTeam(new Team("SV Salching"));
            tournament.AddTeam(new Team("SV Haibach"));
            tournament.AddTeam(new Team("TSV Bogen"));
            tournament.AddTeam(new Team("EC EBRA Aiterhofen"));
            if (generate_9_Teams)
                tournament.AddTeam(new Team("EC Welchenberg"));

            tournament.CreateGames();

            return tournament;

        }
    }
}
