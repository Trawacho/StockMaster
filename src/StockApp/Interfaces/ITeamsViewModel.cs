using StockApp.BaseClasses;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StockApp.Interfaces
{
    public interface ITeamsViewModel
    {
        Team SelectedTeam { get; set; }
        Player SelectedPlayer { get; set; }

        ObservableCollection<Team> Teams { get; }
        ObservableCollection<Player> Players { get; }
        ICommand AddTeamCommand { get; }
        ICommand RemoveTeamCommand { get; }
        ICommand AddPlayerCommand { get; }
        ICommand RemovePlayerCommand { get; }
        ICommand PrintQuittungenCommand { get; }
    }
}
