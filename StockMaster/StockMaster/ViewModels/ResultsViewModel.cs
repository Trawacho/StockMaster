using StockMaster.BaseClasses;
using StockMaster.Commands;
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
        Team SelectedTeam { get; set; }
        List<PointPerTeamAndGame> PointsOfSelectedTeam { get; }

        ICommand SavePointsFromTeamCommand { get; }
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

        private Team _selectedTeam;

        public Team SelectedTeam
        {
            get
            {
                return _selectedTeam;
            }
            set
            {
                if (_selectedTeam == value) return;

                _selectedTeam = value;
                SetPointsOfSelectedTeam();
                RaisePropertyChanged();
            }
        }

        private void SetPointsOfSelectedTeam()
        {
            this.PointsOfSelectedTeam = new List<PointPerTeamAndGame>();
            if (SelectedTeam != null)
            {
                foreach (var game in SelectedTeam.Games.OrderBy(g => g.GameNumberOverAll))
                {
                    PointsOfSelectedTeam.Add(
                        new PointPerTeamAndGame()
                        {
                            GameNumber = game.GameNumberOverAll,

                            StockPunkte = (SelectedTeam == game.TeamA)
                                            ? game.Turns.First(t => t.Number == 1).PointsTeamA
                                            : game.Turns.First(t => t.Number == 1).PointsTeamB,

                            StockPunkteGegner = (SelectedTeam == game.TeamA)
                                            ? game.Turns.First(t => t.Number == 1).PointsTeamB
                                            : game.Turns.First(t => t.Number == 1).PointsTeamA,
                            Gegner = (SelectedTeam == game.TeamA)
                                            ? game.TeamB.TeamName
                                            : game.TeamA.TeamName,

                            IsPauseGame = game.IsPauseGame
                        }
                        );
                }
            }

            RaisePropertyChanged(nameof(PointsOfSelectedTeam));
        }

        public List<PointPerTeamAndGame> PointsOfSelectedTeam { get; set; }

        #region Commands

        private ICommand savePointsFromTeamCommand;
        public ICommand SavePointsFromTeamCommand
        {
            get
            {
                return savePointsFromTeamCommand ?? (savePointsFromTeamCommand = new RelayCommand(
                    (p) =>
                    {
                        foreach (var item in PointsOfSelectedTeam)
                        {
                            var game = SelectedTeam.Games.First(g => g.GameNumberOverAll == item.GameNumber);
                            game.AddTurn1_Value(SelectedTeam, item.StockPunkte, item.StockPunkteGegner);
                        }
                    }));
            }
        }

        #endregion //Commands


    } //class ResultsViewModel

    public class ResultsDesignViewModel : IResultsViewModel
    {
        private readonly Tournament tournament;

        public ResultsDesignViewModel()
        {
            this.tournament = new Tournament();
        }

        public ObservableCollection<Team> Teams
        {
            get
            {
                return new ObservableCollection<Team>(tournament.Teams);
            }
        }
        public Team SelectedTeam { get; set; }

        public List<PointPerTeamAndGame> PointsOfSelectedTeam { get; set; }

        public ICommand SavePointsFromTeamCommand { get; }
    } //ResultsDesignViewModel

    public class PointPerTeamAndGame : TBaseClass
    {
        private int stockPunkte;
        private int stockPunkteGegner;
        private string gegner;

        public int GameNumber { get; set; }
        public int StockPunkte
        {
            get
            {
                return stockPunkte;
            }
            set
            {
                if (IsPauseGame)
                    stockPunkte = 0;
                else
                    stockPunkte = value;

                RaisePropertyChanged();
            }
        }
        public int StockPunkteGegner
        {
            get
            {
                return stockPunkteGegner;

            }
            set
            {
                if (IsPauseGame)
                    stockPunkteGegner = 0;
                else
                    stockPunkteGegner = value;

                RaisePropertyChanged();
            }
        }
        public bool IsPauseGame { get; set; }
        public string Gegner
        {
            get
            {
                if (IsPauseGame)
                    return "Setzt aus";
                return gegner;

            }
            set => gegner = value;

        }
    }  //PointPerTeamAndGame
}