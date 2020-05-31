using StockMaster.BaseClasses;
using System;

namespace StockMaster.Interfaces
{
    public interface ITournamentViewModel
    {
        string Venue { get; set; }
        string Organizer { get; set; }
        string Operator { get; set; }
        string TournamentName { get; set; }
        DateTime DateOfTournament { get; set; }
        EntryFee EntryFee { get; set; }
        bool IsDirectionOfCourtsFromRightToLeft { get; set; }
        string DirectionOfCourtsDescription { get; }
        Referee Referee { get; set; }
        CompetitionManager CompetitionManager { get; set; }
        ComputingOfficer ComputingOfficer { get; set; }
    }
}
