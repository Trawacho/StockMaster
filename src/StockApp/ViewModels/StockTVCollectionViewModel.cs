using StockApp.BaseClasses;
using System.Collections.ObjectModel;

namespace StockApp.ViewModels
{
    internal class StockTVCollectionViewModel : BaseViewModel
    {
        private readonly StockTVs _stockTVs;

        public StockTVCollectionViewModel(ref StockTVs stockTVs)
        {
            this.StockTVCollection = new ObservableCollection<StockTV>();
            this._stockTVs = stockTVs;
            this._stockTVs.StockTVCollectionAdded += StockTVs_StockTVCollectionAdded;
            this._stockTVs.StockTVCollectionRemoved += StockTVs_StockTVCollectionRemoved;

            foreach (StockTV item in _stockTVs)
            {
                item.StockTVSettingsChanged += StockTV_StockTVSettingsChanged;
                this.StockTVCollection.Add(item);
            }
            this.StockTVCollection.Sort();

        }

        private void StockTV_StockTVSettingsChanged(object sender, StockTVSettingsChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() => StockTVCollection.Sort());
        }

        private void StockTVs_StockTVCollectionRemoved(object sender, StockTVCollectionChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (StockTV item in _stockTVs)
                {
                    item.StockTVSettingsChanged -= StockTV_StockTVSettingsChanged;
                }
                StockTVCollection.Clear();
                foreach (StockTV item in _stockTVs)
                {
                    item.StockTVSettingsChanged += StockTV_StockTVSettingsChanged;
                    this.StockTVCollection.Add(item);
                    this.StockTVCollection.Sort();
                }
            });
        }

        private void StockTVs_StockTVCollectionAdded(object sender, StockTVCollectionChangedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                e.StockTV.StockTVSettingsChanged += StockTV_StockTVSettingsChanged;
                this.StockTVCollection.Add(e.StockTV);
                this.StockTVCollection.Sort();
            });
        }

        public ObservableCollection<StockTV> StockTVCollection { get; private set; }

        public bool Sync { get; set; }

    }

    internal class StockTVCollectionDesignViewModel
    {
        public StockTVCollectionDesignViewModel()
        {
            this.StockTVCollection = new ObservableCollection<StockTV>();
            this.StockTVCollection.Add(new StockTV("StockTV-1", "10.10.10.1"));
            this.StockTVCollection.Add(new StockTV("StockTV-2", "10.10.10.2"));
            this.StockTVCollection.Add(new StockTV("StockTV-3", "10.10.10.3"));
            this.StockTVCollection.Add(new StockTV("StockTV-4", "10.10.10.4"));
        }
        public ObservableCollection<StockTV> StockTVCollection { get; private set; }
        public bool Sync { get; set; }
    }
}