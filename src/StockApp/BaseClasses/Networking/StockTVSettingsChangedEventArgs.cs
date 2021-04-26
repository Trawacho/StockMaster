using System;

namespace StockApp.BaseClasses
{
    public delegate void StockTVSettingsChangedHandler(object sender, StockTVSettingsChangedEventArgs stockTVSettings);

    public class StockTVSettingsChangedEventArgs : EventArgs
    {

        public StockTVSettingsChangedEventArgs()
        {

        }

        public StockTVSettingsChangedEventArgs(StockTVSettings tVSettings) : this()
        {
            TVSettings = tVSettings;
        }

        public StockTVSettings TVSettings { get; private set; }
    }
}