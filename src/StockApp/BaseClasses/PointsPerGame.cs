namespace StockApp.BaseClasses
{
    public class PointsPerGame : TBaseClass
    {
        private readonly Game game;

        public PointsPerGame(Game game)
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
