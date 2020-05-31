using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockMaster.Output.Wertungskarte
{
    public class SpiegelFeld : Border
    {
        public FontFamily FntFamily { get; set; }

        public TextBlock Textblock { get; set; }

        public SpiegelFeld(string Text, int RotateAngle = 0)
        {
            FntFamily = new FontFamily("Bahnschrift");

            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
            Margin = new Thickness(-1, -1, 0, 0);

            Textblock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = FntFamily,
                FontSize = 10.0,
                FontStretch = FontStretches.UltraCondensed,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Normal,
                LayoutTransform = new RotateTransform(RotateAngle),
                Text = Text
            };

            Child = Textblock;
        }

        public SpiegelFeld(Brush brush)
        {
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
            Margin = new Thickness(-1, -1, 0, 0);
            Background = brush;
        }

    }
}
