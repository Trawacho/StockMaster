using StockMaster.Converters;
using System.Windows;
using System.Windows.Controls;

namespace StockMaster.Output.TurnCards
{
    public class SpiegelHeaderGrid : SpiegelGrid
    {
        public SpiegelHeaderGrid(bool kehren8):base(kehren8)
        {
            //Two Rows
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(PixelConverter.CmToPx(0.60)) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(PixelConverter.CmToPx(0.60)) });

            int colCounter = 0;
            int kehrenColSpan = kehren8 ? 8 : 7;

            #region Texte  Moarschaft

            //Borders
            SpiegelFeld BorderSpiel = new SpiegelFeld("Bahn", 270);
            SetRowSpan(BorderSpiel, 2);
            SetColumn(BorderSpiel, colCounter);
            SetRow(BorderSpiel, 0);
            Children.Add(BorderSpiel);
            colCounter++;

            SpiegelFeld BorderBahn = new SpiegelFeld("Gegner", 270);
            SetRowSpan(BorderBahn, 2);
            SetColumn(BorderBahn, colCounter);
            SetRow(BorderSpiel, 0);
            Children.Add(BorderBahn);
            colCounter++;
       
            SpiegelFeld BorderAnspiel = new SpiegelFeld("Anspiel", 270);
            SetRowSpan(BorderAnspiel, 2);
            SetColumn(BorderAnspiel, colCounter);
            SetRow(BorderSpiel, 0);
            Children.Add(BorderAnspiel);
            colCounter++;
          
            SpiegelFeld BorderKehre = new SpiegelFeld("K e h r e n");
            SetColumnSpan(BorderKehre, kehrenColSpan);
            SetColumn(BorderKehre, colCounter);
            SetRow(BorderKehre, 0);
            Children.Add(BorderKehre);

            SpiegelFeld BorderKehre1 = new SpiegelFeld("1");
            SetColumn(BorderKehre1, colCounter);
            SetRow(BorderKehre1, 1);
            Children.Add(BorderKehre1);
            colCounter++;
   
            SpiegelFeld BorderKehre2 = new SpiegelFeld("2");
            SetColumn(BorderKehre2, colCounter);
            SetRow(BorderKehre2, 1);
            Children.Add(BorderKehre2);
            colCounter++;
         
            SpiegelFeld BorderKehre3 = new SpiegelFeld("3");
            SetColumn(BorderKehre3, colCounter);
            SetRow(BorderKehre3, 1);
            Children.Add(BorderKehre3);
            colCounter++;
        
            SpiegelFeld BorderKehre4 = new SpiegelFeld("4");
            SetColumn(BorderKehre4, colCounter);
            SetRow(BorderKehre4, 1);
            Children.Add(BorderKehre4);
            colCounter++;

            SpiegelFeld BorderKehre5 = new SpiegelFeld("5");
            SetColumn(BorderKehre5, colCounter);
            SetRow(BorderKehre5, 1);
            Children.Add(BorderKehre5);
            colCounter++;

            SpiegelFeld BorderKehre6 = new SpiegelFeld("6");
            SetColumn(BorderKehre6, colCounter);
            SetRow(BorderKehre6, 1);
            Children.Add(BorderKehre6);
            colCounter++;

            SpiegelFeld BorderKehre7 = new SpiegelFeld("7");
            SetColumn(BorderKehre7, colCounter);
            SetRow(BorderKehre7, 1);
            Children.Add(BorderKehre7);
            colCounter++;
            if (kehren8)
            {
                SpiegelFeld BorderKehre8 = new SpiegelFeld("8");
                SetColumn(BorderKehre8, colCounter);
                SetRow(BorderKehre8, 1);
                Children.Add(BorderKehre8);
                colCounter++;
            }

            SpiegelFeld BorderSumme = new SpiegelFeld("Summe", 0);
            BorderSumme.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSumme, colCounter);
            SetRowSpan(BorderSumme, 2);
            Children.Add(BorderSumme);
            colCounter++;

            SpiegelFeld BorderStrafSumme = new SpiegelFeld("Straf-\r\npunkte", 0);
            SetColumn(BorderStrafSumme, colCounter);
            SetRowSpan(BorderStrafSumme, 2);
            Children.Add(BorderStrafSumme);
            colCounter++;

            SpiegelFeld BorderPunkte = new SpiegelFeld("Gewinn-\r\npunkte", 0);
            BorderPunkte.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkte, colCounter);
            SetRowSpan(BorderPunkte, 2);
            Children.Add(BorderPunkte);
            colCounter++;

            #endregion

            colCounter++;


            #region Texte Gegner

            SpiegelFeld BorderGegner = new SpiegelFeld("Spiel", 270);
            SetRowSpan(BorderGegner, 2);
            SetColumn(BorderGegner, colCounter);
            Children.Add(BorderGegner);
            colCounter++;

            SpiegelFeld BorderKehreG = new SpiegelFeld("K e h r e n");
            SetColumnSpan(BorderKehreG, kehrenColSpan);
            SetColumn(BorderKehreG, colCounter);
            SetRow(BorderKehreG, 0);
            Children.Add(BorderKehreG);

            SpiegelFeld BorderKehre1G = new SpiegelFeld("1");
            SetColumn(BorderKehre1G, colCounter);
            SetRow(BorderKehre1G, 1);
            Children.Add(BorderKehre1G);
            colCounter++;

            SpiegelFeld BorderKehre2G = new SpiegelFeld("2");
            SetColumn(BorderKehre2G, colCounter);
            SetRow(BorderKehre2G, 1);
            Children.Add(BorderKehre2G);
            colCounter++;

            SpiegelFeld BorderKehre3G = new SpiegelFeld("3");
            SetColumn(BorderKehre3G, colCounter);
            SetRow(BorderKehre3G, 1);
            Children.Add(BorderKehre3G);
            colCounter++;

            SpiegelFeld BorderKehre4G = new SpiegelFeld("4");
            SetColumn(BorderKehre4G, colCounter);
            SetRow(BorderKehre4G, 1);
            Children.Add(BorderKehre4G);
            colCounter++;

            SpiegelFeld BorderKehre5G = new SpiegelFeld("5");
            SetColumn(BorderKehre5G, colCounter);
            SetRow(BorderKehre5G, 1);
            Children.Add(BorderKehre5G);
            colCounter++;

            SpiegelFeld BorderKehre6G = new SpiegelFeld("6");
            SetColumn(BorderKehre6G, colCounter);
            SetRow(BorderKehre6G, 1);
            Children.Add(BorderKehre6G);
            colCounter++;

            SpiegelFeld BorderKehre7G = new SpiegelFeld("7");
            SetColumn(BorderKehre7G, colCounter);
            SetRow(BorderKehre7G, 1);
            Children.Add(BorderKehre7G);
            colCounter++;

            if (kehren8)
            {
                SpiegelFeld BorderKehre8G = new SpiegelFeld("8");
                SetColumn(BorderKehre8G, colCounter);
                SetRow(BorderKehre8G, 1);
                Children.Add(BorderKehre8G);
                colCounter++;
            }

            SpiegelFeld BorderSummeG = new SpiegelFeld("Summe", 0);
            BorderSummeG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSummeG, colCounter);
            SetRowSpan(BorderSummeG, 2);
            Children.Add(BorderSummeG);
            colCounter++;

            SpiegelFeld BorderStrafSummeG = new SpiegelFeld("Straf-\r\npunkte", 0);
            SetColumn(BorderStrafSummeG, colCounter);
            SetRowSpan(BorderStrafSummeG, 2);
            Children.Add(BorderStrafSummeG);
            colCounter++;

            SpiegelFeld BorderPunkteG = new SpiegelFeld("Gewinn-\r\npunkte", 0);
            BorderPunkteG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkteG, colCounter);
            SetRowSpan(BorderPunkteG, 2);
            Children.Add(BorderPunkteG);

            #endregion




        }
    }
}
