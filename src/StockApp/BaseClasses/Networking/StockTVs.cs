using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace StockApp.BaseClasses
{
    public class StockTVs : ICollection<StockTV>, IEnumerable<StockTV>
    {
        #region EventHandler

        public event StockTVCollectionChangedEventHandler StockTVCollectionAdded;
        public event StockTVCollectionChangedEventHandler StockTVCollectionRemoved;

        protected void RaiseStockTVCollectionAdded(StockTV stockTV)
        {
            var handler = StockTVCollectionAdded;
            handler?.Invoke(this, new StockTVCollectionChangedEventArgs(stockTV));
        }
        protected void RaiseStockTVCollectionRemoved(StockTV stockTV)
        {
            var handler = StockTVCollectionRemoved;
            handler?.Invoke(this, new StockTVCollectionChangedEventArgs(stockTV));
        }

        #endregion

        #region private Fields

        private readonly List<StockTV> stockTVs;
        private readonly object _lock = new();

        #endregion

        #region Implement ICollection

        public int Count => ((ICollection<StockTV>)stockTVs).Count;

        public bool IsReadOnly => ((ICollection<StockTV>)stockTVs).IsReadOnly;

        public bool Contains(StockTV item)
        {
            return stockTVs.Contains(item);
        }

        public void CopyTo(StockTV[] array, int arrayIndex)
        {
            stockTVs.CopyTo(array, arrayIndex);
        }

        public int IndexOf(StockTV item)
        {
            return stockTVs.IndexOf(item);
        }

        public void Add(StockTV item)
        {
            lock (_lock)
            {
                if (!stockTVs.Any(s => s.IPAddress == item.IPAddress))
                {
                    stockTVs.Add(item);
                    RaiseStockTVCollectionAdded(item);

                }
            }
        }

        public bool Remove(StockTV item)
        {
            lock (_lock)
            {
                if (stockTVs.Remove(item))
                {
                    RaiseStockTVCollectionRemoved(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Clear()
        {
            stockTVs.Clear();
            RaiseStockTVCollectionRemoved(null);
        }

        IEnumerator<StockTV> IEnumerable<StockTV>.GetEnumerator()
        {
            return stockTVs.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return stockTVs.GetEnumerator();
        }

        public StockTV this[int index] { get => ((IList<StockTV>)stockTVs)[index]; set => ((IList<StockTV>)stockTVs)[index] = value; }


        #endregion

        #region Konstruktor

        public StockTVs()
        {
            stockTVs = new List<StockTV>();
        }

        #endregion


        /// <summary>
        /// Adds new StockTV to collection if parameter fits StockTV-Service
        /// </summary>
        /// <param name="mDnsInfo"></param>
        internal void Add(XServiceInfo mDnsInfo)
        {
            if (!mDnsInfo.DomainName.Labels.Contains("_stockapp") && !mDnsInfo.DomainName.Labels.Contains("_stockpub"))
                return;

            this.Add(new StockTV(mDnsInfo));

            this.First(s => s.IPAddress == mDnsInfo.IPAddress).SetStockTVService(mDnsInfo);
        }

        /// <summary>
        /// Removes StockTVService from StockTV with same IPAddress in Collection. Removes StockTV if no more Service exists
        /// </summary>
        /// <param name="dnsInfo"></param>
        internal void Remove(XServiceInfo dnsInfo)
        {
            lock (_lock)
            {
                var stockTV = this.FirstOrDefault(s => s.IPAddress == dnsInfo.IPAddress);
                if (stockTV == null) return;

                stockTV.RemoveStockTVService(dnsInfo);

                //if (!stockTV.ServicesAvailable())
                //{
                //    this.Remove(stockTV);
                //}
            };
        }
    }
}
