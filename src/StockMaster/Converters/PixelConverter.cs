namespace StockMaster.Converters
{
    public class PixelConverter
    {
        private struct PixelUnitFactor
        {
            public const double Px = 1.0;
            public const double Inch = 96.0;
            public const double Cm = 37.7952755905512;
            public const double Pt = 1.33333333333333;
        }

        public static double CmToPx(double cm)
        {
            return cm * PixelUnitFactor.Cm;
        }

        public static double PxToCm(double px)
        {
            return px / PixelUnitFactor.Cm;
        }
    }
}
