using StockMaster.Converters;
using StockMaster.Output.WertungsKarteBase;
using System.Windows;
using System.Windows.Controls;


namespace StockMaster.Output.WertungskarteStockTV
{
    public class GameSummaryGrid : SpiegelGrid
    {
        public GameSummaryGrid(bool kehren8) : base(kehren8)
        {
            RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(PixelConverter.CmToPx(0.6))
            });

            int startColumn = kehren8 ? 10 : 9;

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

            SpiegelFeld BorderPunkteG = new SpiegelFeld(string.Empty, 0);
            BorderPunkteG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkteG, startColumn);
            Children.Add(BorderPunkteG);


        }
    }
   
}
