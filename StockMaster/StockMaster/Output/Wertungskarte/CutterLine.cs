using StockMaster.Converters;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace StockMaster.Output.Wertungskarte
{
    internal static class Tools
    {
        public static  Line CutterLine()
        {
            return new Line()
            {
                X1 = 0,
                X2 = 1,
                Y1 = 0,
                Y2 = 0,
                Stretch = Stretch.Fill,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Margin = new Thickness(0, PixelConverter.CmToPx(0.75), 0, 0)
            };
        }

        public static Line CutterLineTop()
        {
            return new Line()
            {
                X1 = 0,
                X2 = 1,
                Y1 = 0,
                Y2 = 0,
                Stretch = Stretch.Fill,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Margin = new Thickness(0, 0, 0, PixelConverter.CmToPx(0.75))
            };
        }
    }
}
