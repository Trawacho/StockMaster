using Makaretu.Dns;
using MoreLinq;
using System;
using System.Linq;
using System.Windows.Threading;

namespace StockApp.BaseClasses
{
    internal class XDiscover
    {
        public StockTVs StockTVs { get; set; }

        readonly ServiceDiscovery serviceDiscovery;
        readonly DomainName stockAppDNS = new("_stockapp._tcp.local");
        readonly DomainName stockPubDNS = new("_stockpub._tcp.local");
        DispatcherTimer discoveryTimer;

        public XDiscover()
        {
            StockTVs = new StockTVs();

            serviceDiscovery = new ServiceDiscovery();
            
            serviceDiscovery.ServiceInstanceDiscovered += Discovery_ServiceInstanceDiscovered;
            serviceDiscovery.ServiceInstanceShutdown += Discovery_ServiceInstanceShutdown;
        }

        internal void StartService()
        {
            if (discoveryTimer?.IsEnabled ?? false) return;

            if (discoveryTimer == null)
                discoveryTimer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(2)
                };

            discoveryTimer.Tick += (o, e) =>
            {
                var app = string.Join(".", stockAppDNS.Labels.Take(2));
                var pub = string.Join(".", stockPubDNS.Labels.Take(2));
                serviceDiscovery.QueryServiceInstances(app);
                serviceDiscovery.QueryServiceInstances(pub);
               // System.Diagnostics.Debug.WriteLine($"discovery {app} und {pub}");
            };
            discoveryTimer.Start();
        }

        private void Discovery_ServiceInstanceShutdown(object sender, ServiceInstanceShutdownEventArgs args)
        {
            var dnsInfo = new XServiceInfo(args.ServiceInstanceName,
                                        args.Message.AdditionalRecords.OfType<ARecord>().FirstOrDefault()?.Address.ToString() ?? "",
                                        args.Message.AdditionalRecords.OfType<SRVRecord>().FirstOrDefault()?.Port ?? 0);

            StockTVs.Remove(dnsInfo);
        }



        private void Discovery_ServiceInstanceDiscovered(object sender, ServiceInstanceDiscoveryEventArgs args)
        {
            var dnsInfo = new XServiceInfo(args.ServiceInstanceName,
                                        args.Message.AdditionalRecords.OfType<ARecord>().FirstOrDefault()?.Address.ToString() ?? "",
                                        args.Message.AdditionalRecords.OfType<SRVRecord>().FirstOrDefault()?.Port ?? 0);

            foreach (var item in args.Message.AdditionalRecords.OfType<TXTRecord>())
            {
                foreach (var s in item.Strings)
                {
                    dnsInfo.Informations.Add(s);
                }
            }

            StockTVs.Add(dnsInfo);
        }

        internal void Stop()
        {
            discoveryTimer?.Stop();

            foreach(StockTV s in StockTVs)
            {
               s.Stop();
            }
        }
    }

}
