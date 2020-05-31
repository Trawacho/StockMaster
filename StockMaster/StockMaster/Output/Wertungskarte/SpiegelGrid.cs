using StockMaster.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMaster.Output.Wertungskarte
{
    public class SpiegelGrid : Grid
    {
        public SpiegelGrid(bool kehren8)
        {
            double fixedValuesWidth     = PixelConverter.CmToPx(0.6);
            double turnValuesWidth      = PixelConverter.CmToPx(0.7);
            double sumValueWidth        = PixelConverter.CmToPx(1.2);
            double penaltyValueWidth    = PixelConverter.CmToPx(1.0);
            double pointsValueWidth     = PixelConverter.CmToPx(1.1);
            double spaceValueWidth      = PixelConverter.CmToPx(0.5);
            if (kehren8)
            {
                spaceValueWidth = PixelConverter.CmToPx(0.3);
                turnValuesWidth = PixelConverter.CmToPx(0.65);
                sumValueWidth = PixelConverter.CmToPx(1.0);
                pointsValueWidth = PixelConverter.CmToPx(0.9);
            }
            int lineColumn = kehren8 ? 14 : 13;
            #region ColumnDefinitions

            #region Moarschaft

            //Bahn#
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth), });
            //Gegner
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth), });
            //Anspiel
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth), });
            //Kehre 1 - 7 (8)
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            if (kehren8) ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });

            //Summe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(sumValueWidth), });
            //Strafpunkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(penaltyValueWidth), });
            //Punkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(pointsValueWidth), });
            #endregion

            //Leer-Raum
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(spaceValueWidth), });

            #region Gegner

            //Spiel#
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth), });

            //Kehre 1 - 7
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });
            if (kehren8) ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth), });

            //Summe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(sumValueWidth), });
            //Strafpunkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(penaltyValueWidth), });
            //Punkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(pointsValueWidth), });

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
            SetColumn(line, lineColumn);
            SetRowSpan(line, 2);
            Children.Add(line);
        }
    }
}
