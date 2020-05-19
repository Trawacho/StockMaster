using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Output;
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

        ICommand PrintQuittungenCommand { get; }
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
                    return Teams.Count(t => !t.IsVirtual) < 15;
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
            tournament.RemoveTeam(SelectedTeam);
            //tournament.x_Teams.Remove(SelectedTeam);
            RaisePropertyChanged(nameof(Teams));
        }


        public ICommand AddTeamCommand { get; }
        private void AddTeamAction()
        {
            tournament.AddTeam(new Team()
            {
                TeamName = $"default {tournament.Teams.Count + 1}"
            });
            //tournament.x_Teams.Add(new Team
            //{
            //    TeamName = "default"
            //});
            RaisePropertyChanged(nameof(Teams));
        }

        private ICommand printQuittungenCommand;
        public ICommand PrintQuittungenCommand
        {
            get
            {
                return printQuittungenCommand ?? (printQuittungenCommand = new RelayCommand(
                    (p) =>
                    {
                        var x = new Output.Receipts.Receipt(tournament);
                        PrintPreview printPreview = new PrintPreview();
                        var A4Size = new System.Windows.Size(8 * 96, 11.5 * 96);
                        printPreview.Document = x.CreateReceipts(A4Size);
                        printPreview.ShowDialog();
                    }
                    ));
            }
        }
    }

    public class TeamsDesignviewModel : ITeamsViewModel
    {
        private Tournament t = TournamentExtension.CreateNewTournament(true);
        public TeamsDesignviewModel()
        {
            SelectedTeam = t.Teams[3];
            Teams = new ObservableCollection<Team>(t.Teams);
        }
        public ObservableCollection<Team> Teams { get; }

        public Team SelectedTeam { get; set; }

        public ICommand AddTeamCommand { get; }
        public ICommand RemoveTeamCommand { get; }
        public ICommand PrintQuittungenCommand { get; }
    }
}
