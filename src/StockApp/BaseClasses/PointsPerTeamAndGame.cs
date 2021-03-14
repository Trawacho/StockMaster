namespace StockMaster.BaseClasses
{
    public class PointsPerTeamAndGame : TBaseClass
    {
        private readonly Game game;
        private readonly Team selectedTeam;
        public PointsPerTeamAndGame(Game game, Team selectedTeam)
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
}
