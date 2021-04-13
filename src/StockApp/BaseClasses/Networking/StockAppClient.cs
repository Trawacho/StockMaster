using NetMQ;
using NetMQ.Monitoring;
using NetMQ.Sockets;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StockApp.BaseClasses
{

    public class StockTvMqClient
    {
        private const uint _timeout = 1000;
        private NetMQPoller Poller;
        private RequestSocket Socket;
        private NetMQMonitor Monitor;
        private string IPAddress;
        private int Port;
        private string Identifier;
        NetMQQueue<StockTVCommand> sendQueue;

        #region Konstruktor
        public StockTvMqClient(string ip, int port, string identifier)
        {
            Debug.WriteLine($"Create ZMQ client for {identifier}");
            this.IPAddress = ip;
            this.Port = port;
            this.Identifier = identifier;

            sendQueue = new NetMQQueue<StockTVCommand>();
        }

        #endregion

        #region Functions

        public string GetConnectionString()
        {
            return $"tcp://{IPAddress}:{Port}";
        }

        public void Start()
        {
            if (Socket != null) return;

            Socket = new RequestSocket(GetConnectionString());
            Socket.Options.Identity = Encoding.UTF8.GetBytes(this.Identifier);
            Socket.SendReady += Socket_SendReady;

            Poller = new NetMQPoller() { Socket};

            Monitor = new NetMQMonitor(Socket, $"inproc://{Identifier}.inproc", SocketEvents.All);
            Monitor.EventReceived += Monitor_EventReceived;
            Monitor.StartAsync();

            Poller.RunAsync(Identifier);
        }

        public void Stop()
        {
            Socket.SendReady -= Socket_SendReady; Debug.WriteLine("socket unsubscribe event");
            Socket.Disconnect(GetConnectionString()); Debug.WriteLine("socket disconntet");

            Poller.Stop(); Debug.WriteLine("Poller stopped");
            Poller.Remove(Socket); Debug.WriteLine("Poller removes socket");

            Poller.Dispose(); Debug.WriteLine("Poller dispose");
            Socket.Dispose(); Debug.WriteLine("Socket dispose");
            Monitor.Dispose(); Debug.WriteLine("Monitor dispose");

            Poller = null;
            Socket = null;
            Monitor = null;
        }

        public void AddCommand(StockTVCommand command)
        {
            sendQueue.Enqueue(command);
        }

        #endregion

        public bool IsConnected { get; private set; }

        private void Monitor_EventReceived(object sender, NetMQMonitorEventArgs e)
        {
            Debug.WriteLine($"MONITOR: {e.Address} ..  {e.SocketEvent}");
            switch (e.SocketEvent)
            {
                case SocketEvents.Connected:
                    this.IsConnected = true;
                    

                    break;
                case SocketEvents.ConnectDelayed:
                    break;
                case SocketEvents.ConnectRetried:

                    break;
                case SocketEvents.Listening:
                    break;
                case SocketEvents.BindFailed:
                    break;
                case SocketEvents.Accepted:
                    break;
                case SocketEvents.AcceptFailed:
                    break;
                case SocketEvents.Closed:
                    break;
                case SocketEvents.CloseFailed:
                    break;
                case SocketEvents.Disconnected:
                    this.IsConnected = false;


                    break;
                case SocketEvents.All:
                    break;
                default:
                    break;
            }
        }


        private void Socket_SendReady(object sender, NetMQSocketEventArgs e)
        {
            if (sendQueue.Count > 0)
            {
                if (sendQueue.TryDequeue(out StockTVCommand command, TimeSpan.FromSeconds(50)))
                {
                    e.Socket.SendReady -= Socket_SendReady;
                    e.Socket.SendFrame(command.ToString(), false);
                    Debug.WriteLine($"{Encoding.UTF8.GetString(e.Socket.Options.Identity)} sent successfully {sendQueue.Count}");

                    SocketReceiving(sender, e);

                    e.Socket.SendReady += Socket_SendReady;
                }
            }
        }

        private void SocketReceiving(object sender, NetMQSocketEventArgs e)
        {
            var msg = e.Socket.ReceiveMultipartMessage();
            if (msg.FrameCount == 2)
            {
                if (msg.First().ConvertToString().Equals("Settings"))
                    Debug.WriteLine($"Received from { Encoding.UTF8.GetString(e.Socket.Options.Identity) }: {msg.First().ConvertToString()} --> {msg.Last().ConvertToString()}");
                else if (msg.First().ConvertToString().Equals("Result"))
                    Debug.WriteLine($"Received from { Encoding.UTF8.GetString(e.Socket.Options.Identity) }: {msg.First().ConvertToString()} --> {string.Join(" ", msg.Last().ToByteArray().Select(x => x.ToString()))}");
            }
            else
            {
                Debug.WriteLine($"Received from { Encoding.UTF8.GetString(e.Socket.Options.Identity) }: {string.Join(" ", msg.First().ToByteArray().Select(x => x.ToString()))}");
            }
        }




    }

    public class StockAppClient
    {
        #region Konstruktor
        public StockAppClient()
        {

        }

        public StockAppClient(string ipAddress, int Port) : this()
        {
            this.ConnectionString = $"tcp://{ipAddress}:{Port}";
        }

        #endregion

        private readonly string ConnectionString;
        private const uint _timeout = 5000;

        RequestSocket socket;
        byte[] getBackByteArray;

        public bool Start()
        {
            if (socket != null) return false;
            socket = new RequestSocket(ConnectionString);
            Thread.Sleep(10);

            var success = socket.TrySendFrame(TimeSpan.FromMilliseconds(_timeout), "ALIVE", false);
            if (success)
            {
                if (socket.TryReceiveSignal(TimeSpan.FromMilliseconds(_timeout), out bool signal))
                {
                    Debug.WriteLine($"{ConnectionString} Received Signal after starting Service was {signal}");
                }
                else
                {
                    Debug.WriteLine($"no signal received after starting Service {ConnectionString}");
                    success = true;
                }
            }
            else
            {
                Debug.WriteLine($"{ConnectionString} Sending ALIVE signal was not successfully");
            }

            return success;
        }



        public void Stop()
        {
            if (socket == null) return;

            socket.Dispose();
            socket = null;
        }


        public bool SendCommand(StockTVCommand command, Action<byte[]> action)
        {
            var success = SendCommand(command);
            if (success) action?.Invoke(getBackByteArray);
            return success;
        }

        private bool SendCommand(StockTVCommand command)
        {
            if (socket == null) return false;
            if (!socket.HasOut) return false;

            // using (var socket = new RequestSocket(this.ConnectionString))
            {
                if (!socket.TrySendFrame(TimeSpan.FromMilliseconds(_timeout), command.ToString(), false))
                {
                    return false;
                }

                Debug.Write($"sent: {command}..");

                //expect a Message
                if (command.IsGetCommand)
                {
                    var backMsg = new NetMQMessage(2);

                    if (!socket.TryReceiveMultipartMessage(TimeSpan.FromMilliseconds(_timeout), ref backMsg, 2))
                    {
                        return false;
                    }
                    getBackByteArray = backMsg.Last.ToByteArray();
                    Debug.WriteLine($"..answer received {string.Join(" ", getBackByteArray.Select(x => x.ToString()))}");
                    return true;
                }

                //expact only Signal-OK - Message
                if (socket.TryReceiveSignal(TimeSpan.FromMilliseconds(_timeout), out bool signal))
                {
                    Debug.WriteLine($"{ConnectionString}: back: {signal}");
                    return signal;
                }
                else
                {
                    Debug.WriteLine($"OK-Signal not received");
                    return false;
                }
            }

        }

    }


}
