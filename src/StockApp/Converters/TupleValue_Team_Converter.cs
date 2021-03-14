using StockMaster.BaseClasses;
using System;
using System.Globalization;
using System.Windows.Data;

namespace StockMaster.Converters
{
    class TupleValue_Team_Converter : IValueConverter
    {
        /// <summary>
        /// Returns a string from given ValueTuple as <int,Team> dependings on parameter
        /// 
        /// </summary>
        /// <param name="value">ValueTuple<int, Team></param>
        /// <param name="targetType">string</param>
        /// <param name="parameter">TeamName, Platzierung, SpielPunkte, StockPunkte, StockNote, StockPunkteDifferenz</param>
        /// <param name="culture">unused</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValueTuple<int, Team, bool> t)
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
                    return t.Item3
                        ? $"{t.Item2.SpielPunkte_LIVE.positiv}:{t.Item2.SpielPunkte_LIVE.negativ}"
                        : $"{t.Item2.SpielPunkte.positiv}:{t.Item2.SpielPunkte.negativ}";
                }
                if (p == nameof(t.Item2.StockPunkte))
                {
                    return t.Item3
                        ? $"{t.Item2.StockPunkte_LIVE.positiv}:{t.Item2.StockPunkte_LIVE.negativ}"
                        : $"{t.Item2.StockPunkte.positiv}:{t.Item2.StockPunkte.negativ}";
                }
                if (p == nameof(t.Item2.StockNote))
                {
                    return t.Item3
                        ? t.Item2.StockNote_LIVE.ToString("F3")
                        : t.Item2.StockNote.ToString("F3");
                }
                if (p == nameof(t.Item2.StockPunkteDifferenz))
                {
                    return t.Item3
                        ? t.Item2.StockPunkteDifferenz_LIVE.ToString()
                        : t.Item2.StockPunkteDifferenz.ToString();
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
