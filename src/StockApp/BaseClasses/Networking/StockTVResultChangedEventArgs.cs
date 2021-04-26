using System;

namespace StockApp.BaseClasses
{
    public delegate void StockTVResultChangedHandler(object sender, StockTVResultChangedEventArgs stockTVResult);

    public class StockTVResultChangedEventArgs : EventArgs
    {
        public StockTVResultChangedEventArgs()
        {

        }

        public StockTVResultChangedEventArgs(StockTVResult tVResult) : this()
        {
            TVResult = tVResult;
        }

        public StockTVResult TVResult { get; private set; }
    }
}