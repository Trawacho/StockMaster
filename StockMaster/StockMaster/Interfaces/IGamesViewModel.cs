﻿using StockMaster.BaseClasses;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StockMaster.Interfaces
{
    public interface IGamesViewModel
    {
        int NumberOfCourts { get; }
        int NumberOfGameRounds { get; set; }
        ObservableCollection<Team> Teams { get; }
        bool TwoPauseGames { get; set; }
        bool ConcatRoundsOnOutput { get; set; }
        bool TeamNameOnTurnCards { get; set; }
        bool Is8KehrenSpiel { get; set; }
        ICommand RemoveAllGamesCommand { get; }
        ICommand CreateGamesCommand { get; }
        ICommand PrintTurnCardsCommand { get; }
    }
}