using NetMQ;
using NetMQ.Sockets;
using System;

namespace StockApp.BaseClasses
{

    public class SubscriberClient:IDisposable
    {
        public class ShimHandler : IShimHandler
        {
            private PairSocket shim;
            private NetMQPoller poller;
            private SubscriberSocket resultSubscriber;
            private SubscriberSocket aliveSubscriber;
            private readonly string resultTopicString = "ResultInfo";
            private readonly string aliveTopicString = "Alive";
            private readonly string connectionString;
            private StockTVResultMessage resultInfo;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="connectionString">e.g. tcp://192.168.100.130:4748</param>
            public ShimHandler(string connectionString, StockTVResultMessage resultInfo)
            {
                this.connectionString = connectionString;
                this.resultInfo = resultInfo;
            }

            public void Initialise(object state)
            {
            }

            public void Run(PairSocket shim)
            {
                using (resultSubscriber = new SubscriberSocket())
                using (aliveSubscriber = new SubscriberSocket())
                {
                    resultSubscriber.Options.ReceiveHighWatermark = 1000;
                    resultSubscriber.ReceiveReady += OnSubscriberReady;
                    resultSubscriber.Connect(connectionString);
                    resultSubscriber.Subscribe(resultTopicString);

                    aliveSubscriber.Options.ReceiveHighWatermark = 1000;
                    aliveSubscriber.ReceiveReady += AliveSubscriber_ReceiveReady;
                    aliveSubscriber.Connect(connectionString);
                    aliveSubscriber.Subscribe(aliveTopicString);

                    this.shim = shim;
                    shim.ReceiveReady += OnShimReady;
                    shim.SignalOK();
                    poller = new NetMQPoller { shim, resultSubscriber, aliveSubscriber };
                    poller.Run();
                }
            }

            private void AliveSubscriber_ReceiveReady(object sender, NetMQSocketEventArgs e)
            {
                if (e.IsReadyToReceive)
                {
                    string topic = e.Socket.ReceiveFrameString();
                    if (topic.Equals("Alive"))
                    {
                        resultInfo.SetHeartBeat();
                    }
                }
            }

            private void OnSubscriberReady(object sender, NetMQSocketEventArgs e)
            {
                //Check TOPIC of Message and set Value if TOPIC fits
                if (resultSubscriber.ReceiveFrameString() == "ResultInfo")
                    resultInfo.SetValues(resultSubscriber.ReceiveFrameBytes());
            }

            private void OnShimReady(object sender, NetMQSocketEventArgs e)
            {
                string command = e.Socket.ReceiveFrameString();
                if (command == NetMQActor.EndShimMessage)
                {
                    poller.Stop();
                }
            }
        }

        private NetMQActor actor;
        private bool disposedValue;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip">e.g. 192.168.100.10</param>
        /// <param name="port">4748</param>
        public SubscriberClient(string ip, int port, ref StockTVResultMessage resultInfo) 
        {
            this.IP = ip;
            this.Port = port;
            this.ResultMessage = resultInfo;
        }

        /// <summary>
        /// e.g. tcp://192.168.100.130:4748
        /// </summary>
        public string Address => String.Format("tcp://{0}:{1}", IP, Port);
        public string IP { get; private set; }
        public int Port { get; private set; }

        public StockTVResultMessage ResultMessage
        {
            get; set;
        }

        public void Start()
        {
            if (actor != null)
                return;
            actor = NetMQActor.Create(new ShimHandler(Address, ResultMessage));
        }

        public void Stop()
        {
            if (actor != null)
            {
                actor.Dispose();
                actor = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Stop();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SubscriberClient()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
