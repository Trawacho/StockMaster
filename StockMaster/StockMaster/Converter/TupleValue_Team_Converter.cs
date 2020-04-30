using StockMaster.BaseClasses;
using System;
using System.Globalization;
using System.Windows.Data;

namespace StockMaster.Converter
{
    class TupleValue_Team_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tt = value.GetType();

            if (value is ValueTuple<int, Team> t)
            {
                string p = (string)parameter;
                if (p == nameof(t.Item2.TeamName))
                {
                    return t.Item2.TeamName;
                }
                if (p == "Platzierung")
                {
                    return t.Item1.ToString();
                }
                if (p == nameof(t.Item2.SpielPunkte))
                {
                    return $"{t.Item2.SpielPunkte.positiv}:{t.Item2.SpielPunkte.negativ}";
                }
                if(p == nameof(t.Item2.StockPunkte))
                {
                    return $"{t.Item2.StockPunkte.positiv}:{t.Item2.StockPunkte.negativ}";
                }
                if (p == nameof(t.Item2.StockNote))
                {
                    return t.Item2.StockNote.ToString("F2");
                }
                if (p == nameof(t.Item2.StockPunkteDifferenz))
                {
                    return t.Item2.StockPunkteDifferenz.ToString();
                }
            }

            return "not def";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
