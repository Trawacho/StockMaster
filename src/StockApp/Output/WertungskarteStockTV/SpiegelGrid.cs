using StockApp.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockApp.Output.WertungskarteStockTV
{
    public class SpiegelGrid : Grid
    {
        public SpiegelGrid(bool kehren8)
        {
            double fixedValuesWidth     = PixelConverter.CmToPx(0.55);
            double turnValuesWidth      = kehren8 ? PixelConverter.CmToPx(0.65) : PixelConverter.CmToPx(0.7);
            double sumValueWidth        = kehren8 ? PixelConverter.CmToPx(1.2)  : PixelConverter.CmToPx(1.2);
            double pointsValueWidth     = kehren8 ? PixelConverter.CmToPx(1.2)  :PixelConverter.CmToPx(1.1);
            double spaceValueWidth      = kehren8 ? PixelConverter.CmToPx(0.3)  : PixelConverter.CmToPx(0.5);
            double inputColorValueWidth = PixelConverter.CmToPx(1.4);
            int lineColumn = kehren8 ? 14 : 13;

            #region ColumnDefinitions

            #region Moarschaft

            //Bahn#
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth) });
            //Gegner
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth) });
            //Anspiel
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth) });
            //Eingabe Farbe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(inputColorValueWidth) });
            //Kehre 1 - 7 (8)
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            if (kehren8) ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });

            //Summe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(sumValueWidth) });
            
            //Punkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(pointsValueWidth) });
            #endregion

            //Leer-Raum
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(spaceValueWidth) });

            #region Gegner

            //Spiel#
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(fixedValuesWidth) });

            //Kehre 1 - 7
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });
            if (kehren8) ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(turnValuesWidth) });

            //Summe
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(sumValueWidth) });
           
            //Punkte
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(pointsValueWidth) });

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
