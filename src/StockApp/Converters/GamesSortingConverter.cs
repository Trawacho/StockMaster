using StockApp.BaseClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace StockApp.Converters
{
    public class GamesSortingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<Game> games)
            {
                return games.OrderBy(a => a.RoundOfGame).ThenBy(b => b.GameNumber).ToList();
            }
            throw new NotSupportedException("Type of value is not IEnumerable");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
