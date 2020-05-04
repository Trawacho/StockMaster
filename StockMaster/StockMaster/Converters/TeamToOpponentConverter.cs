using StockMaster.BaseClasses;
using System;
using System.Globalization;
using System.Windows.Data;

namespace StockMaster.Converters
{
    public class TeamToOpponentConverter : IMultiValueConverter
    {
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values[0] is Team t)
            {
                if(values[1] is Game g)
                {
                    return g.GetOpponent(t).ToString();
                }
            }
            throw new NotImplementedException();
        }

       

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
