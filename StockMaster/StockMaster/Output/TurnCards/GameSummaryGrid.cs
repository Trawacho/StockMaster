using StockMaster.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StockMaster.Output.TurnCards
{
    public class GameSummaryGrid : SpiegelGrid
    {
        public GameSummaryGrid(bool kehren8):base(kehren8)
        {
            RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(PixelConverter.CmToPx(0.6))
            });

            int startColumn = kehren8 ? 9: 8;
            
            var textBlockGesamt = new TextBlockGesamt();
            SetColumn(textBlockGesamt, startColumn);
            SetColumnSpan(textBlockGesamt, 2);
            Children.Add(textBlockGesamt);
            startColumn += 2;

            SpiegelFeld BorderSumme = new SpiegelFeld(string.Empty, 0);
            BorderSumme.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSumme, startColumn);
            Children.Add(BorderSumme);
            startColumn++;

            SpiegelFeld BorderStrafSumme = new SpiegelFeld(string.Empty, 0);
            SetColumn(BorderStrafSumme, startColumn);
            Children.Add(BorderStrafSumme);
            startColumn++;

            SpiegelFeld BorderPunkte = new SpiegelFeld(string.Empty, 0);
            BorderPunkte.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkte, startColumn);
            Children.Add(BorderPunkte);

            startColumn = kehren8 ? startColumn + 9 : startColumn + 8;

            var textBlockGesamtG = new TextBlockGesamt();
            SetColumn(textBlockGesamtG, startColumn);
            SetColumnSpan(textBlockGesamtG, 2);
            Children.Add(textBlockGesamtG);
            startColumn += 2;

            SpiegelFeld BorderSummeG = new SpiegelFeld(string.Empty, 0);
            BorderSummeG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSummeG, startColumn);
            Children.Add(BorderSummeG);
            startColumn++;

            SpiegelFeld BorderStrafSummeG = new SpiegelFeld(string.Empty, 0);
            SetColumn(BorderStrafSummeG, startColumn);
            Children.Add(BorderStrafSummeG);
            startColumn++;

            SpiegelFeld BorderPunkteG = new SpiegelFeld(string.Empty, 0);
            BorderPunkteG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkteG, startColumn);
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
