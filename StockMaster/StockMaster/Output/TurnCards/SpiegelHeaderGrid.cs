using StockMaster.Converters;
using System.Windows;
using System.Windows.Controls;

namespace StockMaster.Output.TurnCards
{
    public class SpiegelHeaderGrid : SpiegelGrid
    {
        public SpiegelHeaderGrid()
        {
            //Two Rows
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(pxConverter.CmToPx(0.60)) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(pxConverter.CmToPx(0.60)) });
           

            #region Texte  Moarschaft

            //Borders
            SpiegelFeld BorderSpiel = new SpiegelFeld("Bahn", 270);
            SetRowSpan(BorderSpiel, 2);
            SetColumn(BorderSpiel, 0);
            SetRow(BorderSpiel, 0);
            Children.Add(BorderSpiel);

            SpiegelFeld BorderBahn = new SpiegelFeld("Gegner", 270);
            SetRowSpan(BorderBahn, 2);
            SetColumn(BorderBahn, 1);
            SetRow(BorderSpiel, 0);
            Children.Add(BorderBahn);
       
            SpiegelFeld BorderAnspiel = new SpiegelFeld("Anspiel", 270);
            SetRowSpan(BorderAnspiel, 2);
            SetColumn(BorderAnspiel, 2);
            SetRow(BorderSpiel, 0);
            Children.Add(BorderAnspiel);
          
            SpiegelFeld BorderKehre = new SpiegelFeld("K e h r e n");
            SetColumnSpan(BorderKehre, 7);
            SetColumn(BorderKehre, 3);
            SetRow(BorderKehre, 0);
            Children.Add(BorderKehre);

            SpiegelFeld BorderKehre1 = new SpiegelFeld("1");
            SetColumn(BorderKehre1, 3);
            SetRow(BorderKehre1, 1);
            Children.Add(BorderKehre1);
   
            SpiegelFeld BorderKehre2 = new SpiegelFeld("2");
            SetColumn(BorderKehre2, 4);
            SetRow(BorderKehre2, 1);
            Children.Add(BorderKehre2);
         
            SpiegelFeld BorderKehre3 = new SpiegelFeld("3");
            SetColumn(BorderKehre3, 5);
            SetRow(BorderKehre3, 1);
            Children.Add(BorderKehre3);
        
            SpiegelFeld BorderKehre4 = new SpiegelFeld("4");
            SetColumn(BorderKehre4, 6);
            SetRow(BorderKehre4, 1);
            Children.Add(BorderKehre4);

            SpiegelFeld BorderKehre5 = new SpiegelFeld("5");
            SetColumn(BorderKehre5, 7);
            SetRow(BorderKehre5, 1);
            Children.Add(BorderKehre5);

            SpiegelFeld BorderKehre6 = new SpiegelFeld("6");
            SetColumn(BorderKehre6, 8);
            SetRow(BorderKehre6, 1);
            Children.Add(BorderKehre6);

            SpiegelFeld BorderKehre7 = new SpiegelFeld("7");
            SetColumn(BorderKehre7, 9);
            SetRow(BorderKehre7, 1);
            Children.Add(BorderKehre7);

            SpiegelFeld BorderSumme = new SpiegelFeld("Summe", 0);
            BorderSumme.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSumme, 10);
            SetRowSpan(BorderSumme, 2);
            Children.Add(BorderSumme);

            SpiegelFeld BorderStrafSumme = new SpiegelFeld("Straf-\r\npunkte", 0);
            SetColumn(BorderStrafSumme, 11);
            SetRowSpan(BorderStrafSumme, 2);
            Children.Add(BorderStrafSumme);

            SpiegelFeld BorderPunkte = new SpiegelFeld("Gewinn-\r\npunkte", 0);
            BorderPunkte.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkte, 12);
            SetRowSpan(BorderPunkte, 2);
            Children.Add(BorderPunkte);

            #endregion



            #region Texte Gegner

            SpiegelFeld BorderGegner = new SpiegelFeld("Spiel", 270);
            SetRowSpan(BorderGegner, 2);
            SetColumn(BorderGegner, 14);
            Children.Add(BorderGegner);

            SpiegelFeld BorderKehreG = new SpiegelFeld("K e h r e n");
            SetColumnSpan(BorderKehreG, 7);
            SetColumn(BorderKehreG, 15);
            SetRow(BorderKehreG, 0);
            Children.Add(BorderKehreG);

            SpiegelFeld BorderKehre1G = new SpiegelFeld("1");
            SetColumn(BorderKehre1G, 15);
            SetRow(BorderKehre1G, 1);
            Children.Add(BorderKehre1G);

            SpiegelFeld BorderKehre2G = new SpiegelFeld("2");
            SetColumn(BorderKehre2G, 16);
            SetRow(BorderKehre2G, 1);
            Children.Add(BorderKehre2G);

            SpiegelFeld BorderKehre3G = new SpiegelFeld("3");
            SetColumn(BorderKehre3G, 17);
            SetRow(BorderKehre3G, 1);
            Children.Add(BorderKehre3G);

            SpiegelFeld BorderKehre4G = new SpiegelFeld("4");
            SetColumn(BorderKehre4G, 18);
            SetRow(BorderKehre4G, 1);
            Children.Add(BorderKehre4G);

            SpiegelFeld BorderKehre5G = new SpiegelFeld("5");
            SetColumn(BorderKehre5G, 19);
            SetRow(BorderKehre5G, 1);
            Children.Add(BorderKehre5G);

            SpiegelFeld BorderKehre6G = new SpiegelFeld("6");
            SetColumn(BorderKehre6G, 20);
            SetRow(BorderKehre6G, 1);
            Children.Add(BorderKehre6G);

            SpiegelFeld BorderKehre7G = new SpiegelFeld("7");
            SetColumn(BorderKehre7G, 21);
            SetRow(BorderKehre7G, 1);
            Children.Add(BorderKehre7G);

            SpiegelFeld BorderSummeG = new SpiegelFeld("Summe", 0);
            BorderSummeG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderSummeG, 22);
            SetRowSpan(BorderSummeG, 2);
            Children.Add(BorderSummeG);

            SpiegelFeld BorderStrafSummeG = new SpiegelFeld("Straf-\r\npunkte", 0);
            SetColumn(BorderStrafSummeG, 23);
            SetRowSpan(BorderStrafSummeG, 2);
            Children.Add(BorderStrafSummeG);

            SpiegelFeld BorderPunkteG = new SpiegelFeld("Gewinn-\r\npunkte", 0);
            BorderPunkteG.Textblock.FontWeight = FontWeights.Bold;
            SetColumn(BorderPunkteG, 24);
            SetRowSpan(BorderPunkteG, 2);
            Children.Add(BorderPunkteG);

            #endregion




        }
    }
}
