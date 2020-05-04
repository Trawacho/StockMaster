using StockMaster.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMaster.Output.TurnCards
{
    public class SpiegelGrid : Grid
    {
        public SpiegelGrid()
        {
            #region ColumnDefinitions

            #region Moarschaft

            //Bahn#
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.6)), });
            //Gegner
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.6)), });
            //Anspiel
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.6)), });
            //Kehre 1 - 7
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });

            //Summe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(1.2)), });
            //Strafpunkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(1.0)), });
            //Punkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(1.1)), });
            #endregion

            //Leer-Raum
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.5)), });

            #region Gegner

            //Spiel#
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.6)), });

            //Kehre 1 - 7
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(0.7)), });

            //Summe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(1.2)), });
            //Strafpunkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(1.0)), });
            //Punkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(PixelConverter.CmToPx(1.1)), });

            #endregion

            #endregion

            Line line = new Line
            {
                StrokeThickness = 0.75,
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = 0.1,
                Stroke = Brushes.Black,
                Stretch = Stretch.Fill,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            SetColumn(line, 13);
            SetRowSpan(line, 2);
            Children.Add(line);
        }
    }
}
