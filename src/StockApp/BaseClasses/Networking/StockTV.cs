using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StockApp.BaseClasses
{
    public class StockTV : IEquatable<StockTV>
    {
        #region EventHandler

        private event StockTVServiceChangedEventHandler StockTVServiceAdded;
        protected void RaiseStockTVServiceAdded(StockTVService service)
        {
            var handler = StockTVServiceAdded;
            handler?.Invoke(this, new StockTVServiceChangedEventArgs(service));
        }

        private event StockTVServiceChangedEventHandler StockTVServiceRemoved;
        protected void RaiseStockTVServiceRemove(StockTVService service)
        {
            var handler = StockTVServiceRemoved;
            handler?.Invoke(this, new StockTVServiceChangedEventArgs(service));
        }


        public event StockTVResultChangedHandler StockTVResultChanged;
        protected void RaiseStockTVResultChanged()
        {
            var handler = StockTVResultChanged;
            handler?.Invoke(this, new StockTVResultChangedEventArgs(TVResult));
        }


        public event StockTVSettingsChangedHandler StockTVSettingsChanged;
        protected void RaiseStockTVSettingsChanged()
        {
            var handler = StockTVSettingsChanged;
            handler?.Invoke(this, new StockTVSettingsChangedEventArgs(TVSettings));
        }


        public event StockTVOnlineChangedEventHandler StockTVOnlineChanged;
        protected void RaiseStockTVOnlineChanged()
        {
            var handler = StockTVOnlineChanged;
            handler?.Invoke(this, this.IsOnline);
        }

        #endregion

        #region IEquatable-Implementation

        /// <summary>
        /// True if Hostname is equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(StockTV other)
        {
            return this.HostName == other.HostName;
        }

        #endregion

        #region Fields and Properties

        private StockTVService ApplicationService;
        private StockTVAppClient appClient;

        private StockTVService PublisherService;
        private StockTVSubscriberClient subscriberClient;

        private readonly object _lock = new();

        private readonly List<string> _infos;
        public IEnumerable<string> Informationen => _infos;


        public string IPAddress { private set; get; }
        public string HostName { private set; get; }
        public DomainName DomainName { private set; get; }

        public DateTime LastMDnsUpdate { get; private set; }

        private bool _isOnline;
        public bool IsOnline
        {
            get => _isOnline;
            private set
            {
                if (_isOnline == value) return;

                _isOnline = value;
                RaiseStockTVOnlineChanged();
            }
        }

        #endregion

        #region Konstruktor
        public StockTV()
        {
            _infos = new List<string>();
            tvSettings = StockTVSettings.GetDefault(GameModis.Training);
        }

        /// <summary>
        /// for design only
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="ipaddress"></param>
        public StockTV(string hostname, string ipaddress) : this()
        {
            this.HostName = hostname;
            this.IPAddress = ipaddress;
        }

        /// <summary>
        /// Default-Konstruktor
        /// </summary>
        /// <param name="mDnsInfo"></param>
        public StockTV(MDnsServiceInfo mDnsInfo) : this()
        {
            IPAddress = mDnsInfo.IPAddress;
            HostName = mDnsInfo.DomainName.Labels.First();
            DomainName = mDnsInfo.DomainName;
            UpdateInfos(mDnsInfo.Informations);
        }

        #endregion

        #region StockTV mDNS-Service Implementation

        private void UpdateInfos(IEnumerable<string> infos)
        {
            lock (_lock)
            {
                LastMDnsUpdate = DateTime.Now;
                foreach (var i in infos)
                {
                    if (!_infos.Contains(i))
                        _infos.Add(i);
                }
            }
        }

        public void SetStockTVService(MDnsServiceInfo mDnsInfo)
        {
            UpdateInfos(mDnsInfo.Informations);

            var service = new StockTVService(mDnsInfo);
            if (service == null) return;
            lock (_lock)
            {
                if (service.IsApplicationService)
                {
                    if (ApplicationService == null || !ApplicationService.Equals(service))
                    {
                        ApplicationService = service;
                        RaiseStockTVServiceAdded(service);

                        if (appClient == null)
                        {
                            appClient = new StockTVAppClient(IPAddress, service.Port, HostName);
                            appClient.StockTVOnlineChanged += AppClient_StockTVOnlineChanged;
                            appClient.Start();
                        }
                    }
                }
                else if (service.IsPublisherService)
                {
                    if (PublisherService == null || !PublisherService.Equals(service))
                    {
                        PublisherService = service;
                        RaiseStockTVServiceAdded(service);

                        if (subscriberClient == null)
                        {
                            subscriberClient = new StockTVSubscriberClient(IPAddress, PublisherService.Port);
                            subscriberClient.StockTVResultChanged += SubscriberClient_StockTVResultChanged;
                            subscriberClient.Start();
                        }
                    }
                }
            }
        }

       

        public void RemoveStockTVService(MDnsServiceInfo mDnsInfo)
        {
            var service = new StockTVService(mDnsInfo);

            if (service.IsApplicationService)
            {
                ApplicationService = null;
                StopAppClient();
            }
            else if (service.IsPublisherService)
            {
                PublisherService = null;
                StopSubscriberClient();
            }

            RaiseStockTVServiceRemove(service);
        }

        #endregion


        #region StockTVResult

        /// <summary>
        /// Called from Event within <see cref="subscriberClient"/> 
        /// </summary>
        /// <param name="stockTVResult"></param>
        private void SubscriberClient_StockTVResultChanged(object sender, StockTVResult stockTVResult)
        {
            //Create a copy to use, not reference
            TVResult = new StockTVResult(subscriberClient.StockTVResult);
        }

        /// <summary>
        /// Returns <see cref="StockTVResult"/> after sending a request to StockTV
        /// </summary>
        /// <returns></returns>
        public void TVResultGet()
        {
            appClient?.AddCommand(StockTVCommand.GetResultCommand((byteArray) => TVResult = new StockTVResult(byteArray)));
        }

        public void TVResultReset()
        {
            appClient?.AddCommand(StockTVCommand.ResetCommand());
        }

        private StockTVResult tvResult;
        /// <summary>
        /// Updated through the <see cref="subscriberClient"/>
        /// </summary>
        public StockTVResult TVResult
        {
            get => tvResult;
            private set
            {
                if (tvResult == value) return;

                tvResult = value;
                RaiseStockTVResultChanged();
            }
        }

        #endregion


        #region StockTVSettings

        private readonly StockTVSettings tvSettings;

        public StockTVSettings TVSettings
        {
            get => tvSettings;
            //private set
            //{
            //    Debug.WriteLine($"Try to SET TVSettings {this.IPAddress}");
            //    if (tvSettings == value) return;

            //    tvSettings = value;

            //    Debug.WriteLine($"SET TVSettings {this.IPAddress}");

            //    RaiseStockTVSettingsChanged();
            //}
        }


        public void TVSettingsGet()
        {
            appClient?.AddCommand(StockTVCommand.GetSettingsCommand((byteArray) =>
            {
                TVSettings.SetValues(byteArray);
                Debug.WriteLine($"Update TVSettings {this.IPAddress}");
                RaiseStockTVSettingsChanged();
            }));
        }

        public void TVSettingsSend()
        {
            appClient?.AddCommand(StockTVCommand.SendSettingsCommand(TVSettings));
        }



        #endregion

        private void AppClient_StockTVOnlineChanged(object sender, bool IsOnline)
        {
            this.IsOnline = IsOnline;
            if (this.IsOnline)
                TVSettingsGet();
        }


        public void SendTeamNames(IEnumerable<StockTVBegegnung> begegnungen)
        {
            appClient?.AddCommand(StockTVCommand.SendBegegnungenCommand(begegnungen));
        }

        /// <summary>
        /// Stops the <see cref="subscriberClient"/> and removes the event <see cref="StockTVResult_StockTVResultChanged(StockTVResult)"/>
        /// </summary>
        private void StopSubscriberClient()
        {
            if (subscriberClient != null)
            {
                subscriberClient.Stop();
                subscriberClient.StockTVResultChanged -= SubscriberClient_StockTVResultChanged;
                subscriberClient = null;
            }
        }

        /// <summary>
        /// Stops the <see cref="appClient"/>
        /// </summary>
        private void StopAppClient()
        {
            appClient.StockTVOnlineChanged -= AppClient_StockTVOnlineChanged;
            appClient?.Stop();
            appClient = null;
            IsOnline = false;
        }

        /// <summary>
        /// Stops the <seealso cref="subscriberClient"/> and the <see cref="appClient"/>
        /// </summary>
        public void Stop()
        {
            StopSubscriberClient();
            StopAppClient();
        }
    }
}
