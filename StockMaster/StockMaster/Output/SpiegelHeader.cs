using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using StockMaster.BaseClasses;

namespace StockMaster.Output
{
    public class SpiegelHeader : SpiegelGrid
    {
        private FontFamily fnt = new FontFamily("Consolas");

        public string StartNummer { get; set; }
        public SpiegelHeader(Team team)
        {
            StartNummer = team.StartNumber.ToString();

            Margin = new Thickness(0, 0, 0, 5);

            TextBlock textBlockStartnummer = new TextBlock()
            {
                Text = string.Concat("Nr. ", StartNummer),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                FontFamily = fnt
            };
            SetColumnSpan(textBlockStartnummer, 4);
            SetColumn(textBlockStartnummer, 0);
            Children.Add(textBlockStartnummer);

            TextBlock textBlockMoarschaft = new TextBlock()
            {
                Text = "Moarschaft:",
                FontWeight = FontWeights.Normal,
                FontSize = 12,
                FontFamily = fnt,
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            SetColumnSpan(textBlockMoarschaft, 3);
            SetColumn(textBlockMoarschaft, 3);
            Children.Add(textBlockMoarschaft);

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
            SetColumnSpan(line, 7);
            SetColumn(line, 6);
            Children.Add(line);

            TextBlock textBlockGegner = new TextBlock()
            {
                Text = "Gegner",
                FontWeight = FontWeights.Normal,
                FontSize = 12,
                FontFamily = fnt,
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            SetColumnSpan(textBlockGegner, 7);
            SetColumn(textBlockGegner, 14);
            Children.Add(textBlockGegner);
        }

        public SpiegelHeader()
        {
            
        }
    }
}
