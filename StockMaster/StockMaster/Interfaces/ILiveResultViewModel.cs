using StockMaster.BaseClasses;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StockMaster.Interfaces
{
    public interface ILiveResultViewModel
    {
        bool IsLive { get; set; }
        bool ShowDifferenz { get; set; }
        bool ShowStockPunkte { get; set; }
        ObservableCollection<(int Platzierung, Team Team, bool isLive)> Ergebnisliste { get; }
        ICommand RefreshCommand { get; }
        ICommand CloseCommand { get; }
    }
}
