using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace StockMaster.BaseClasses
{
    public class SerializableTournamentSet
    {
        public SerializableTournamentSet()
        {

        }

        public void SetTournament(Tournament tournament)
        {
            this.Teams = tournament.Teams.ToList();
            this.Games = tournament.GetAllGames()
                                 .Distinct<Game>()
                                 .OrderBy(r => r.RoundOfGame)
                                 .ThenBy(g => g.GameNumber)
                                 .ThenBy(c => c.CourtNumber)
                                 .ToList();
            this.Venue = tournament.Venue;
            this.Operator = tournament.Operator;
            this.Organizer = tournament.Organizer;
            this.DateOfTournament = tournament.DateOfTournament;
            this.EntryFee = tournament.EntryFee;
            this.Is8KehrenSpiel = tournament.Is8KehrenSpiel;
            this.IsDirectionOfCourtsFromRightToLeft = tournament.IsDirectionOfCourtsFromRightToLeft;
            this.IsNumberOfPause2 = tournament.IsNumberOfPause2;
            this.NumberOfGameRounds = tournament.NumberOfGameRounds;
            this.NumberOfTeamsWithNamedPlayerOnResult = tournament.NumberOfTeamsWithNamedPlayerOnResult;
            this.ComputingOffice = tournament.Rechenbüro;
            this.Referee = tournament.Schiedsrichter;
            this.StartOfTeamChange = tournament.StartOfTeamChange;
            this.TournamentName = tournament.TournamentName;
            this.TournamentManager = tournament.Wettbewerbsleiter;
        }
        

        public Tournament GetTournament()
        {
            Tournament tournament = new Tournament
            {
                Venue = this.Venue,
                Operator = this.Operator,
                Organizer = this.Organizer,
                DateOfTournament = this.DateOfTournament,
                EntryFee = this.EntryFee,
                Is8KehrenSpiel = this.Is8KehrenSpiel,
                IsDirectionOfCourtsFromRightToLeft = this.IsDirectionOfCourtsFromRightToLeft,
                IsNumberOfPause2 = this.IsNumberOfPause2,
                NumberOfGameRounds = this.NumberOfGameRounds,
                NumberOfTeamsWithNamedPlayerOnResult = this.NumberOfTeamsWithNamedPlayerOnResult,
                Rechenbüro = this.ComputingOffice,
                Schiedsrichter = this.Referee,
                StartOfTeamChange = this.StartOfTeamChange,
                TournamentName = this.TournamentName,
                Wettbewerbsleiter = this.TournamentManager
            };

            tournament.RemoveAllTeams();
            foreach (var team in Teams)
            {
                tournament.AddTeam(team);
            }

            foreach (var game in Games)
            {
                game.TeamA = Teams.First(t => t.StartNumber == game.StartNumberTeamA);
                game.TeamB = Teams.First(t => t.StartNumber == game.StartNumberTeamB);

                tournament.Teams.First(t => t == game.TeamA).AddGame(game);
                tournament.Teams.First(t => t == game.TeamB).AddGame(game);
            }

            return tournament;
        }

        [XmlElement(Order = 2)]
        public string TournamentName { get; set; }

        [XmlElement(Order = 3)]
        public string Venue { get; set; }

        [XmlElement(Order = 4)]
        public string Operator { get; set; }

        [XmlElement(Order = 5)]
        public string Organizer { get; set; }

        [XmlElement(Order = 6)]
        public DateTime DateOfTournament { get; set; }

        [XmlElement(Order = 7)]
        public EntryFee EntryFee { get; set; }

        [XmlElement(Order = 8)]
        public bool StartOfTeamChange { get; set; }

        [XmlElement(Order = 9)]
        public bool Is8KehrenSpiel { get; set; }

        [XmlElement(Order = 10)]
        public bool IsDirectionOfCourtsFromRightToLeft { get; set; }

        [XmlElement(Order = 11)]
        public bool IsNumberOfPause2 { get; set; }

        [XmlElement(Order = 12)]
        public int NumberOfGameRounds { get; set; }

        [XmlElement(Order = 13)]
        public int NumberOfTeamsWithNamedPlayerOnResult { get; set; }

        [XmlElement(Order = 14)]
        public ComputingOfficer ComputingOffice { get; set; }

        [XmlElement(Order = 15)]
        public Referee Referee { get; set; }

        [XmlElement(Order = 16)]
        public CompetitionManager TournamentManager { get; set; }

        [XmlArray(ElementName = nameof(Teams), Order = 99)]
        [XmlArrayItem(nameof(Team))]
        public List<Team> Teams { get; set; }


        [XmlArray(ElementName = nameof(Games), Order = 100)]
        [XmlArrayItem(nameof(Game))]
        public List<Game> Games { get; set; }


    }
}
