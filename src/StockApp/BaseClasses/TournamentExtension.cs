using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace StockApp.BaseClasses
{
    internal class TournamentExtension
    {
        public static Tournament CreateNewTournament(bool generate_9_Teams = false)
        {
            var tournament = new Tournament
            {
                NumberOfGameRounds = 1,
                TwoPauseGames = false,
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


        public static void Save(Tournament tournament, string filePath)
        {
            var set = new SerializableTournamentSet();
            set.SetTournament(tournament);
            var xmlString = "";

            using (var stringWriter = new StringWriter())
            using (var writer = XmlWriter.Create(stringWriter))
            {
                var serializer = new XmlSerializer(typeof(SerializableTournamentSet));
                serializer.Serialize(writer, set);
                xmlString = stringWriter.ToString();
            }

            File.WriteAllText(filePath, xmlString);
        }

        public static Tournament Load(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            var serializer = new XmlSerializer(typeof(SerializableTournamentSet));
            var set = serializer.Deserialize(reader) as SerializableTournamentSet;
            return set.GetTournament();
        }


    }
}
