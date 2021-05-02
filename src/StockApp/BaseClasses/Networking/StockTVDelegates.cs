﻿namespace StockApp.BaseClasses
{
    public delegate void StockTVOnlineChangedEventHandler(object sender, bool IsOnline);
    public delegate void StockTVCollectionChangedEventHandler(object sender, StockTVCollectionChangedEventArgs e);
    public delegate void StockTVResultChangedHandler(object sender, StockTVResultChangedEventArgs e);
    public delegate void StockTVServiceChangedEventHandler(object sender, StockTVServiceChangedEventArgs e);
    public delegate void StockTVSettingsChangedHandler(object sender, StockTVSettingsChangedEventArgs e);

}
