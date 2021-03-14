﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StockApp.BaseClasses
{
    /// <summary>
    /// Basisklasse, hat INotifyPropertychanged implementiert
    /// </summary>
    public class TBaseClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
