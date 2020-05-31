using StockMaster.BaseClasses;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StockMaster.Interfaces
{
    public interface ILiveResultViewModel
    {
        ObservableCollection<(int Platzierung, Team Team)> Ergebnisliste { get; }
        ICommand RefreshCommand { get; }
        ICommand CloseCommand { get; }
    }
}
