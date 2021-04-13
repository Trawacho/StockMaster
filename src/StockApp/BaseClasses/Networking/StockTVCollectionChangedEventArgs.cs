using System;

namespace StockApp.BaseClasses
{
    public delegate void StockTVCollectionChangedEventHandler(object sender, StockTVCollectionChangedEventArgs e);
    public class StockTVCollectionChangedEventArgs : EventArgs
    {
        public StockTVCollectionChangedEventArgs()
        {

        }
        public StockTVCollectionChangedEventArgs(StockTV stockTV) : this()
        {
            this.StockTV = stockTV;
        }
        public StockTV StockTV { get; private set; }
    }

}