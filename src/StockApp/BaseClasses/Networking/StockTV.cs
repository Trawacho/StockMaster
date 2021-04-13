using Makaretu.Dns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockApp.BaseClasses
{
    public class StockTV : IEquatable<StockTV>
    {
        #region EventHandler

        public event StockTVServiceChangedEventHandler StockTVServiceAdded;
        public event StockTVServiceChangedEventHandler StockTVServiceRemoved;

        protected void RaiseStockTVServiceAdded(StockTVService service)
        {
            var handler = StockTVServiceAdded;
            handler?.Invoke(this, new StockTVServiceChangedEventArgs(service));
        }

        protected void RaiseStockTVServiceRemove(StockTVService service)
        {
            var handler = StockTVServiceRemoved;
            handler?.Invoke(this, new StockTVServiceChangedEventArgs(service));
        }

        #endregion

        #region Fields and Properties

        private readonly object _lock = new();

        private readonly List<string> _infos;

        public string IPAddress { private set; get; }
        public string HostName { private set; get; }
        public DomainName DomainName { get; set; }

        public StockTVService ApplicationService { get; private set; }

        public StockTVService PublisherService { get; private set; }

        public IEnumerable<string> Informationen => _infos;
        public DateTime LastUpdate { get; private set; }

        #endregion

        #region Konstruktor

        /// <summary>
        /// Default-Konstruktor
        /// </summary>
        /// <param name="mDnsInfo"></param>
        public StockTV(XServiceInfo mDnsInfo)
        {
            _infos = new List<string>();
            LastUpdate = DateTime.Now;
            StockTVResultMsg = new StockTVResultMessage();
            StockTVSettings = new StockTVSettings();

            IPAddress = mDnsInfo.IPAddress;
            HostName = mDnsInfo.DomainName.Labels.First();
            DomainName = mDnsInfo.DomainName;
            UpdateInfos(mDnsInfo.Informations);
        }

        #endregion

        #region StockTV Service Implementation

        private void UpdateInfos(IEnumerable<string> infos)
        {
            lock (_lock)
            {
                LastUpdate = DateTime.Now;
                foreach (var i in infos)
                {
                    if (!_infos.Contains(i))
                        _infos.Add(i);
                }
            }
        }

        public void SetStockTVService(XServiceInfo mDnsInfo)
        {
            UpdateInfos(mDnsInfo.Informations);

            var service = new StockTVService(mDnsInfo);
            if (service == null) return;
            lock (_lock)
            {
                if (service.IsApplicationService)
                {
                    if (this.ApplicationService == null || !(this.ApplicationService.Equals(service)))
                    {
                        this.ApplicationService = service;
                        RaiseStockTVServiceAdded(service);

                        if (zmq == null)
                        {
                            this.zmq = new StockTvMqClient(this.IPAddress, service.Port, this.HostName);
                            zmq.Start();
                        }

                    }
                }
                else if (service.IsPublisherService && !(this.PublisherService?.Equals(service) ?? false))
                {
                    this.PublisherService = service;
                    RaiseStockTVServiceAdded(service);
                }
            }
        }

        public void RemoveStockTVService(XServiceInfo mDnsInfo)
        {
            var service = new StockTVService(mDnsInfo);

            if (service.IsApplicationService)
            {
                this.ApplicationService = null;
            }
            else if (service.IsPublisherService)
            {
                this.PublisherService = null;
            }

            RaiseStockTVServiceRemove(service);
        }

        public bool ServicesAvailable()
        {
            return PublisherService != null || ApplicationService != null;
        }

        #endregion

        #region Interface-Implementation

        public void Stop()
        {

            SetSubscriberOffline();
            zmq?.Stop();

        }

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


        SubscriberClient subscriberClient;
        StockTVResultMessage StockTVResultMsg;
        StockTVSettings StockTVSettings;
        //StockAppClient appClient;
        StockTvMqClient zmq;


        public void zmqSend(StockTVCommand command)
        {
            zmq.AddCommand(command);
        }


        public void SetSubscriberOnline()
        {
            if (PublisherService == null) return;
            subscriberClient = new SubscriberClient(this.IPAddress, PublisherService.Port, ref StockTVResultMsg);
            subscriberClient.Start();
            subscriberClient.ResultMessage.ResultInfoChanged += ResultMessage_ResultInfoChanged;
        }

        private void ResultMessage_ResultInfoChanged(object sender)
        {
            this.StockTVResultMsg = ((StockTVResultMessage)sender);
        }

        public void SetSubscriberOffline()
        {
            if (subscriberClient != null)
            {
                subscriberClient.Stop();
                subscriberClient.ResultMessage.ResultInfoChanged -= ResultMessage_ResultInfoChanged;
                subscriberClient = null;
            }
            StockTVResultMsg = null;
        }






    }
}
