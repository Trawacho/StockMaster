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
        Player SelectedPlayer { get; set; }

        ObservableCollection<Team> Teams { get; }
        ObservableCollection<Player> Players { get; }
        ICommand AddTeamCommand { get; }
        ICommand RemoveTeamCommand { get; }

        ICommand AddPlayerCommand { get; }
        ICommand RemovePlayerCommand { get; }
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
        public ObservableCollection<Player> Players
        {
            get
            {
                if (SelectedTeam == null) return null; 
                return new ObservableCollection<Player>(SelectedTeam.Players);
            }
        }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get
            {
                return _selectedTeam ?? (SelectedTeam = Teams.FirstOrDefault());
            }
            set
            {
                if (_selectedTeam == value) return;

                _selectedTeam = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Players));
            }
        }

        private Player _selectedPlayer;
        public Player SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                if (_selectedPlayer == value) return;

                _selectedPlayer = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RemoveTeamCommand { get; }

        private void RemoveTeamAction()
        {
            tournament.RemoveTeam(SelectedTeam);
            RaisePropertyChanged(nameof(Teams));
        }


        public ICommand AddTeamCommand { get; }
        private void AddTeamAction()
        {
            tournament.AddTeam(new Team(tournament.NumberOfPlayersPerTeam)
            {
                TeamName = $"default {tournament.Teams.Count + 1}"
            });

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

        private ICommand addPlayerCommand;
        public ICommand AddPlayerCommand
        {
            get
            {
                return addPlayerCommand ?? (addPlayerCommand = new RelayCommand(
                    (p) =>
                    {
                        SelectedTeam.AddPlayer();
                        RaisePropertyChanged(nameof(Players));

                    },
                    (p) =>
                     Players?.Count < Team.MaxNumberOfPlayers));
            }
        }

        private ICommand removePlayerCommand;
        public ICommand RemovePlayerCommand
        {
            get
            {
                return removePlayerCommand ?? (removePlayerCommand = new RelayCommand(
                    (p) =>
                    {
                        SelectedTeam.RemovePlayer(SelectedPlayer);
                        RaisePropertyChanged(nameof(Players));
                    },
                    (p) =>
                    Players?.Count > Team.MinNumberOfPlayer 
                    && SelectedPlayer != null));
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
        public ObservableCollection<Player> Players
        {
            get
            {
                return new ObservableCollection<Player>(SelectedTeam.Players);
            }
        }
        public Team SelectedTeam { get; set; }
        public Player SelectedPlayer { get; set; }
        public ICommand AddTeamCommand { get; }
        public ICommand RemoveTeamCommand { get; }

        public ICommand AddPlayerCommand { get; }
        public ICommand RemovePlayerCommand { get; }
        public ICommand PrintQuittungenCommand { get; }
    }
}
