using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Interfaces;
using StockMaster.Output;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    public class TeamsViewModel : BaseViewModel, ITeamsViewModel
    {
        private readonly Tournament tournament;
        public TeamsViewModel(Tournament tournament)
        {
            this.tournament = tournament;
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
                return _selectedPlayer ?? (SelectedPlayer = SelectedTeam?.Players.FirstOrDefault());
            }
            set
            {
                if (_selectedPlayer == value) return;

                _selectedPlayer = value;
                RaisePropertyChanged();
            }
        }

        private ICommand _removeTeamCommand;
        public ICommand RemoveTeamCommand
        {
            get
            {
                return _removeTeamCommand ??= new RelayCommand(
                    (p) =>
                    {
                        tournament.RemoveTeam(SelectedTeam);
                        RaisePropertyChanged(nameof(Teams));
                    },
                (o) =>
                {
                    return SelectedTeam != null;
                }
                );
            }
        }

        private ICommand _addTeamCommand;
        public ICommand AddTeamCommand
        {
            get
            {
                return _addTeamCommand ??= new RelayCommand(
                    (p) =>
                    {
                        tournament.AddTeam(new Team($"default {tournament.Teams.Count + 1}"), true);
                        RaisePropertyChanged(nameof(Teams));
                    },
                    (p) =>
                    {
                        return Teams.Count(t => !t.IsVirtual) < 15;
                    });
            }
        }

        private ICommand printQuittungenCommand;
        public ICommand PrintQuittungenCommand
        {
            get
            {
                return printQuittungenCommand ??= new RelayCommand(
                    (p) =>
                    {
                        var x = new Output.Receipts.Receipt(tournament);
                        PrintPreview printPreview = new PrintPreview();
                        var A4Size = new System.Windows.Size(8 * 96, 11.5 * 96);
                        printPreview.Document = x.CreateReceipts(A4Size);
                        printPreview.ShowDialog();
                    },
                    (p) => Teams.Count > 0
                    );
            }
        }

        private ICommand addPlayerCommand;
        public ICommand AddPlayerCommand
        {
            get
            {
                return addPlayerCommand ??= new RelayCommand(
                    (p) =>
                    {
                        SelectedTeam.AddPlayer();
                        RaisePropertyChanged(nameof(Players));

                    },
                    (p) =>
                     Players?.Count < Team.MaxNumberOfPlayers);
            }
        }

        private ICommand removePlayerCommand;
        public ICommand RemovePlayerCommand
        {
            get
            {
                return removePlayerCommand ??= new RelayCommand(
                    (p) =>
                    {
                        SelectedTeam.RemovePlayer(SelectedPlayer);
                        RaisePropertyChanged(nameof(Players));
                    },
                    (p) =>
                    Players?.Count > Team.MinNumberOfPlayer
                    && SelectedPlayer != null);
            }
        }
    }

    public class TeamsDesignviewModel : ITeamsViewModel
    {
        private readonly Tournament t = TournamentExtension.CreateNewTournament(true);
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
