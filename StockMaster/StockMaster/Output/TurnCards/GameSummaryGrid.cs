using StockMaster.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockMaster.Output.TurnCards
{
    public class GameSummaryGrid : SpiegelGrid
    {
        public GameSummaryGrid()
        {
            RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(PixelConverter.CmToPx(0.6))
            });
            
            var textBlockGesamt = new TextBlockGesamt();
            SetColumn(textBlockGesamt, 8);
            SetColumnSpan(textBlockGesamt, 2);
            Children.Add(textBlockGesamt);

            SpiegelFeld BorderSumme = new SpiegelFeld(string.Empty, 0);
            BorderSumme.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSumme, 10);
            Children.Add(BorderSumme);

            SpiegelFeld BorderStrafSumme = new SpiegelFeld(string.Empty, 0);
            SetColumn(BorderStrafSumme, 11);
            Children.Add(BorderStrafSumme);

            SpiegelFeld BorderPunkte = new SpiegelFeld(string.Empty, 0);
            BorderPunkte.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkte, 12);
            Children.Add(BorderPunkte);



            var textBlockGesamtG = new TextBlockGesamt();
            SetColumn(textBlockGesamtG, 20);
            SetColumnSpan(textBlockGesamtG, 2);
            Children.Add(textBlockGesamtG);

            SpiegelFeld BorderSummeG = new SpiegelFeld(string.Empty, 0);
            BorderSummeG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSummeG, 22);
            Children.Add(BorderSummeG);

            SpiegelFeld BorderStrafSummeG = new SpiegelFeld(string.Empty, 0);
            SetColumn(BorderStrafSummeG, 23);
            Children.Add(BorderStrafSummeG);

            SpiegelFeld BorderPunkteG = new SpiegelFeld(string.Empty, 0);
            BorderPunkteG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkteG, 24);
            Children.Add(BorderPunkteG);

            
        }
    }
    public class TextBlockGesamt : TextBlock
    {
        public TextBlockGesamt()
        {
            Text = "Gesamt";
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            FontFamily = new FontFamily("Bahnschrift");
            FontSize = 12.0;
            FontStretch = FontStretches.Normal;
            TextAlignment = TextAlignment.Center;
            FontWeight = FontWeights.Bold;
        }
    }
}
