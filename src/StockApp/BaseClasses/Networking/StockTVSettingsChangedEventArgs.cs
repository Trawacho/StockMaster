using System;

namespace StockApp.BaseClasses
{

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