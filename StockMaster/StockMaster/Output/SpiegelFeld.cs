using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockMaster.Output
{
    public class SpiegelFeld : Border
    {

        public FontFamily fntFamily { get; set; }


        public TextBlock Textblock { get; set; }
               
        public SpiegelFeld(string Text, int RotateAngle = 0)
        {
            fntFamily = new FontFamily("Bahnschrift");

            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
            Margin = new Thickness(-1, -1, 0, 0);

            Textblock = new TextBlock();

            Textblock.HorizontalAlignment = HorizontalAlignment.Center;
            Textblock.VerticalAlignment = VerticalAlignment.Center;
            Textblock.FontFamily = fntFamily;
            Textblock.FontSize = 10.0;
            Textblock.FontStretch = FontStretches.UltraCondensed;
            Textblock.TextAlignment = TextAlignment.Center;
            Textblock.FontWeight = FontWeights.Normal;
            Textblock.LayoutTransform = new RotateTransform(RotateAngle);
            Textblock.Text = Text;

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
