using StockMaster.BaseClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMaster.Output.Wertungskarte
{
    public class SpiegelHeader : SpiegelGrid
    {
        private readonly FontFamily fnt = new FontFamily("Consolas");

        public string StartNummer { get; set; }
        public SpiegelHeader(Team team, bool printTeamName, bool kehren8) : base(kehren8)
        {
            int columnSpan = kehren8 ? 8 : 7;

            StartNummer = team.StartNumber.ToString();

            Margin = new Thickness(0, 0, 0, 5);

            //StartNummer
            TextBlock textBlockStartnummer = new TextBlock()
            {
                Text = string.Concat("Nr. ", StartNummer),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                FontFamily = fnt
            };
            SetColumnSpan(textBlockStartnummer, 3);
            SetColumn(textBlockStartnummer, 0);
            Children.Add(textBlockStartnummer);

            //Moarschaft
            TextBlock textBlockMoarschaft = new TextBlock()
            {
                Text = "Moarschaft:",
                FontWeight = FontWeights.Normal,
                FontSize = 12,
                FontFamily = fnt,
                TextAlignment = TextAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            SetColumnSpan(textBlockMoarschaft, 3);
            SetColumn(textBlockMoarschaft, 3);
            Children.Add(textBlockMoarschaft);

            //TeamName oder nur Linie
            if (printTeamName)
            {
                TextBlock textBlockTeamName = new TextBlock()
                {
                    Text = team.TeamName,
                    FontWeight = FontWeights.Normal,
                    FontSize = 14,
                    FontFamily = fnt,
                    TextAlignment = TextAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                SetColumnSpan(textBlockTeamName, columnSpan);
                SetColumn(textBlockTeamName, 6);
                Children.Add(textBlockTeamName);
            }
            else
            {
                Line line = new Line()
                {
                    StrokeThickness = 1,
                    Stroke = Brushes.Black,
                    Stretch = Stretch.Fill,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    X1 = 0,
                    Y1 = 0,
                    X2 = 5,
                    Y2 = 0
                };
                SetColumnSpan(line, columnSpan);
                SetColumn(line, 6);
                Children.Add(line);
            }

            //Gegener
            TextBlock textBlockGegner = new TextBlock()
            {
                Text = "Gegner",
                FontWeight = FontWeights.Normal,
                FontSize = 12,
                FontFamily = fnt,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            SetColumnSpan(textBlockGegner, columnSpan);
            SetColumn(textBlockGegner, 14);
            Children.Add(textBlockGegner);
        }

        public SpiegelHeader(Team team, bool printTeamName, bool kehren8, int numberOfRound) : this(team, printTeamName, kehren8)
        {
            TextBlock textBlockRound = new TextBlock()
            {
                Text = string.Concat("Runde: ", numberOfRound),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                FontFamily = fnt,
                TextAlignment = TextAlignment.Right
            };
            SetColumnSpan(textBlockRound, 2);
            SetColumn(textBlockRound, this.ColumnDefinitions.Count - 2);
            Children.Add(textBlockRound);
        }
    }
}
