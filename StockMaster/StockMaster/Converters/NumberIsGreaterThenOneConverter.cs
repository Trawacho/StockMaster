using System;
using System.Globalization;
using System.Windows.Data;

namespace StockMaster.Converters
{
    public class NumberIsGreaterThenOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int i)
            {
                return i > 1;
            }
            return new ArgumentException("value must be an integer");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
