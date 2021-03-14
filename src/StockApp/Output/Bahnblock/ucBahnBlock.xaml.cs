using StockApp.BaseClasses;
using System.Windows.Controls;

namespace StockApp.Output.Bahnblock
{
    /// <summary>
    /// Interaction logic for ucBahnBlock.xaml
    /// </summary>
    public partial class ucBahnBlock : UserControl
    {
        public ucBahnBlock(Game game)
        {
            InitializeComponent();

            this.BahnInfo.Content = $"Bahn: {game.CourtNumber}";
            this.SpielInfo.Content = $"Spiel: {game.GameNumber}";
            this.DurchgangInfo.Content = $"Runde: {game.RoundOfGame}";
            this.AnspielInfo.Content = $"Anspiel: {game.StartingTeam.StartNumber}";
            this.StartingTeamName.Content = $"{game.StartingTeam.TeamName}";
            this.StartTeamNumber.Content = $"{game.StartingTeam.StartNumber}";
            this.GegnerTeamName.Content = $"{game.NotStartingTeam.TeamName}";
            this.GegnerTeamNumber.Content = $"{game.NotStartingTeam.StartNumber}";
            this.UnterschriftGegnerTeam.Content = $"Unterschrift {game.NotStartingTeam.TeamName}";
            this.UnterschriftStartingTeam.Content = $"Unterschrift {game.StartingTeam.TeamName}";
        }
    }
}
