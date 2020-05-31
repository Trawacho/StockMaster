using StockMaster.BaseClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StockMaster.Interfaces
{

    public interface IResultsViewModel
    {
        ObservableCollection<Team> Teams { get; }
        ObservableCollection<Game> Games { get; }
        Team SelectedTeam { get; set; }
        Game SelectedGame { get; set; }
        List<PointsPerTeamAndGame> PointsOfSelectedTeam { get; }
        List<PointsPerGame> PointsPerGameList { get; set; }
        int NumberOfTeamsWithNamedPlayers { get; set; }

    } //IResultsViewModel
}