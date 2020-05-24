using StockMaster.BaseClasses;
using StockMaster.Commands;
using StockMaster.Output;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace StockMaster.ViewModels
{
    interface IResultsViewModel
    {
        ObservableCollection<Team> Teams { get; }
        ObservableCollection<Game> Games { get; }
        Team SelectedTeam { get; set; }
        Game SelectedGame { get; set; }
        List<PointPerTeamAndGame> PointsOfSelectedTeam { get; }
        List<PointPerGame> PointsPerGameList { get; set; }
        int NumberOfTeamsWithNamedPlayers { get; set; }

    } //IResultsViewModel

    public class ResultsViewModel : BaseViewModel, IResultsViewModel
    {
        private readonly Tournament tournament;

        public ResultsViewModel(Tournament tournament)
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

        public ObservableCollection<Game> Games
        {
            get
            {
                return new ObservableCollection<Game>(tournament.GetAllGames()
                                                                .Where(g => g.IsNotPauseGame)
                                                                .OrderBy(o => o.GameNumberOverAll)
                                                                .GroupBy(x => x.GameNumberOverAll)
                                                                .Select(group => group.First()));
            }
        }

        private Team _selectedTeam;
        public Team SelectedTeam
        {
            get
            {
                return _selectedTeam ?? (SelectedTeam = Teams[0]);
            }
            set
            {
                if (_selectedTeam == value) return;

                _selectedTeam = value;


                SetPointsOfSelectedTeam();
                RaisePropertyChanged();
            }
        }

        private Game _selectedGame;
        public Game SelectedGame
        {
            get
            {
                return _selectedGame ?? (SelectedGame = Games[0]);
            }
            set
            {
                if (_selectedGame == value) return;

                _selectedGame = value;

                SetPointsPerGame();
                RaisePropertyChanged();
            }
        }

        private void SetPointsPerGame()
        {

            this.PointsPerGameList = new List<PointPerGame>();
            foreach (var game in tournament.GetAllGames()
                                        .Where(g => g.RoundOfGame == SelectedGame.RoundOfGame
                                                 && g.GameNumber == SelectedGame.GameNumber
                                                 && g.IsNotPauseGame)
                                        .Distinct<Game>()
                                        .OrderBy(o => o.CourtNumber))
            {
                PointsPerGameList.Add(new PointPerGame(game));
            }

            RaisePropertyChanged(nameof(PointsPerGameList));
        }

        private void SetPointsOfSelectedTeam()
        {
            this.PointsOfSelectedTeam = new List<PointPerTeamAndGame>();
            foreach (var game in SelectedTeam.Games.OrderBy(g => g.GameNumberOverAll))
            {
                PointsOfSelectedTeam.Add(new PointPerTeamAndGame(game, SelectedTeam));
            }

            RaisePropertyChanged(nameof(PointsOfSelectedTeam));
        }

        public List<PointPerTeamAndGame> PointsOfSelectedTeam { get; set; }

        public List<PointPerGame> PointsPerGameList { get; set; }

        public int NumberOfTeamsWithNamedPlayers
        {
            get
            {
                return tournament.NumberOfTeamsWithNamedPlayerOnResult;
            }
            set
            {
                if (tournament.NumberOfTeamsWithNamedPlayerOnResult == value) return;

                tournament.NumberOfTeamsWithNamedPlayerOnResult = value;
                RaisePropertyChanged();
            }
        }

        private ICommand _printErgebnislisteCommand;
        public ICommand PrintErgebnislisteCommand
        {
            get
            {
                return _printErgebnislisteCommand ?? (_printErgebnislisteCommand = new RelayCommand(
                    (p) =>
                    {
                        var x = new Output.Results.Result(tournament);
                        var printPreview = new PrintPreview();
                        var A4Size = new System.Windows.Size(8 * 96, 11.5 * 96);
                        printPreview.Document = x.CreateResult(A4Size);
                        printPreview.ShowDialog();

                    }));
            }
        }

    } //class ResultsViewModel

    public class ResultsDesignViewModel : IResultsViewModel
    {
        private readonly Tournament tournament;

        public ResultsDesignViewModel()
        {
            this.tournament = new Tournament();
            this.SelectedGame = tournament.GetAllGames().First(g => g.GameNumberOverAll == 1);
            //this.SelectedTeam = tournament.Teams[0];
        }

        public ObservableCollection<Team> Teams
        {
            get
            {
                return new ObservableCollection<Team>(tournament.Teams);
            }
        }

        public ObservableCollection<Game> Games
        {
            get
            {
                return new ObservableCollection<Game>(tournament.GetAllGames()
                    .Where(g => g.IsNotPauseGame)
                    .OrderBy(o => o.GameNumberOverAll)
                    .ThenBy(p => p.CourtNumber));
            }
        }

        public Team SelectedTeam { get; set; }

        public Game SelectedGame { get; set; }

        public List<PointPerTeamAndGame> PointsOfSelectedTeam { get; set; }
        public List<PointPerGame> PointsPerGameList { get; set; }

        public ICommand PrintErgebnislisteCommand { get; }
        public int NumberOfTeamsWithNamedPlayers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    } //ResultsDesignViewModel

    public class PointPerTeamAndGame : TBaseClass
    {
        private readonly Game game;
        private readonly Team selectedTeam;
        public PointPerTeamAndGame(Game game, Team selectedTeam)
        {
            this.game = game;
            this.selectedTeam = selectedTeam;
        }

        public int GameNumber
        {
            get
            {
                return game.GameNumberOverAll;
            }
        }
        public int StockPunkte
        {
            get
            {
                return (selectedTeam == game.TeamA)
                            ? game.MasterTurn.PointsTeamA
                            : game.MasterTurn.PointsTeamB;
            }
            set
            {
                if (IsPauseGame) return;

                if (selectedTeam == game.TeamA)
                {
                    game.MasterTurn.PointsTeamA = value;
                }
                else
                {
                    game.MasterTurn.PointsTeamB = value;

                }

                RaisePropertyChanged();
            }
        }
        public int StockPunkteGegner
        {
            get
            {
                return (selectedTeam == game.TeamA)
                                    ? game.MasterTurn.PointsTeamB
                                    : game.MasterTurn.PointsTeamA;
            }
            set
            {
                if (IsPauseGame) return;
                
                if (selectedTeam == game.TeamA)
                {
                    game.MasterTurn.PointsTeamB = value;
                }
                else
                {
                    game.MasterTurn.PointsTeamA = value;

                }

                RaisePropertyChanged();
            }
        }
        public bool IsPauseGame
        {
            get
            {
                return game.IsPauseGame;
            }
        }

        public string Gegner
        {
            get
            {
                if (IsPauseGame)
                    return "Setzt aus";

                if (selectedTeam == game.TeamA)
                    return game.TeamB.TeamName;

                return game.TeamA.TeamName;
            }
        }

    }  //PointPerTeamAndGame

    public class PointPerGame : TBaseClass
    {
        private readonly Game game;
        public PointPerGame(Game game)
        {
            this.game = game;
        }


        public int Bahn
        {
            get
            {
                return game.CourtNumber;
            }
        }
        public string TeamNameA
        {
            get
            {
                return game.TeamA.TeamName;
            }
        }

        public int StockPunkteA
        {
            get
            {
                return game.MasterTurn.PointsTeamA;
            }
            set
            {
                game.MasterTurn.PointsTeamA = value;
                RaisePropertyChanged();
            }
        }


        public string TeamNameB
        {
            get
            {
                return game.TeamB.TeamName;
            }
        }


        public int StockPunkteB
        {
            get
            {
                return game.MasterTurn.PointsTeamB;
            }
            set
            {
                game.MasterTurn.PointsTeamB = value;
                RaisePropertyChanged();
            }
        }

    } //PointPerGame
}