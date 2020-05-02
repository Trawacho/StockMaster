using StockMaster.BaseClasses;
using StockMaster.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public interface ITeamsViewModel
    {
        Team SelectedTeam { get; set; }
        ObservableCollection<Team> Teams { get; }
        ICommand AddTeamCommand { get; }
        ICommand RemoveTeamCommand { get; }
    }

    public class TeamsViewModel : BaseViewModel, ITeamsViewModel
    {
        private readonly Tournament tournament;
        public TeamsViewModel(Tournament tournament)
        {
            this.tournament = tournament;
            this.AddTeamCommand = new RelayCommand(
                (p) =>
                {
                    AddTeamAction();
                },
                (o) =>
                {
                    return Teams.Count(t=> !t.IsVirtual) < 15;
                }
                );

            this.RemoveTeamCommand = new RelayCommand(
                (p) =>
                {
                    RemoveTeamAction();
                },
                (o) =>
                {
                    return SelectedTeam != null;
                }
                );

            tournament.Teams.PropertyChanged += Teams_PropertyChanged;
        }

        private void Teams_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Teams));
        }

        public ObservableCollection<Team> Teams
        {
            get
            {
                return new ObservableCollection<Team>(tournament.Teams.Where(t => !t.IsVirtual));
            }
        }

        public Team SelectedTeam { get; set; }

        public ICommand RemoveTeamCommand { get; }

        private void RemoveTeamAction()
        {
            tournament.Teams.Remove(SelectedTeam);
           // RaisePropertyChanged(nameof(Teams));
        }


        public ICommand AddTeamCommand { get; }
        private void AddTeamAction()
        {
            tournament.Teams.Add(new Team
            {
                TeamName = "default"
            });
          //  RaisePropertyChanged(nameof(Teams));
        }
    }

    public class TeamsDesignviewModel : ITeamsViewModel
    {
        public TeamsDesignviewModel()
        {

        }
        public ObservableCollection<Team> Teams { get; }

        public Team SelectedTeam { get; set; }

        public ICommand AddTeamCommand { get; }
        public ICommand RemoveTeamCommand { get; }
    }
}
