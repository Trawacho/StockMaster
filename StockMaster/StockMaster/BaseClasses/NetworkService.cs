using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace StockMaster.BaseClasses
{
    public class NetworkService
    {
        private UdpClient udpClient;
        private UdpState state;
        private readonly Tournament tournament;
        private readonly Action _CallBackAfterUpdateAction;

        public event EventHandler StartStopStateChanged;
        protected virtual void OnStartStopStateChanged(NetworkServiceEventArgs e)
        {
            StartStopStateChanged?.Invoke(this, e);
        }


        private class UdpState
        {
            public UdpClient udpClient;
            public IPEndPoint ipEndPoint;
            public IAsyncResult result;
        }


        public NetworkService(Tournament tournament, Action callBackAfterUpdateAction)
        {
            this.tournament = tournament;
            this._CallBackAfterUpdateAction = callBackAfterUpdateAction;
        }

        public void Start()
        {
            if (udpClient == null)
            {
                udpClient = new UdpClient(Settings.Instanze.BroadcastPort);
                udpClient.Client.ReceiveTimeout = 500;
                udpClient.EnableBroadcast = true;
                udpClient.Client.Blocking = false;

            }
            if (state == null)
            {
                state = new UdpState()
                {
                    udpClient = udpClient,
                    ipEndPoint = new IPEndPoint(IPAddress.Any, Settings.Instanze.BroadcastPort),
                };
            }

            ReceiveBroadcast();
            OnStartStopStateChanged(new NetworkServiceEventArgs(true));
        }

        public void Stop()
        {
            udpClient.Dispose();
            udpClient = null;
            state.udpClient = null;
            state = null;

            OnStartStopStateChanged(new NetworkServiceEventArgs(false));
        }

        public bool IsRunning()
        {
            return (udpClient != null);
        }

        public void SwitchStartStopState()
        {
            if (IsRunning())
                Stop();
            else
                Start();
        }

        private void ReceiveBroadcast()
        {
            state.result = state.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), state);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient u = ((UdpState)ar.AsyncState).udpClient;
                IPEndPoint e = ((UdpState)ar.AsyncState).ipEndPoint;
                IAsyncResult r = ((UdpState)ar.AsyncState).result;

                byte[] receiveBytes = u?.EndReceive(ar, ref e);
                if (receiveBytes?.Length > 1)
                {
                    DeSerialize(DeCompress(receiveBytes));
                }
                else
                {
                    if (receiveBytes?[0] == byte.MaxValue)
                    {
                        tournament.ResetAllGames();
                    }
                }

                r = u?.BeginReceive(new AsyncCallback(ReceiveCallback), state);
            }
            catch (ObjectDisposedException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        static byte[] DeCompress(byte[] data)
        {
            if (data == null)
                return null;

            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (var datastream = new DeflateStream(input, CompressionMode.Decompress))
            {
                datastream.CopyTo(output);
            }

            return output.ToArray();
        }

        void DeSerialize(byte[] data)
        {
            /* 
             * 03 15 09 21 07 09 15
             *
             * Aufbau eines Datagramms: 
             * Im ersten Byte steht die Bahnnummer ( 03 )
             * In jedem weiteren Byte kommen die laufenden Spiele, erst der Wert der linken Mannschaft,
             * dann der Wert der rechten Mannschaft
             * 
             */

            if (data == null)
                return;

            try
            {

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{data.Length} -- Bahnnummer:{data[0]} -- {string.Join("-", data)}");
#endif

                byte bahnNumber = data[0];
                var courtGames = tournament.GetGamesOfCourt(bahnNumber);

                //Das erste Byte aus dem Array wird nicht mehr benötigt. Daten in ein neues Array kopieren
                byte[] newData = new byte[data.Length - 1];
                Array.Copy(data, 1, newData, 0, data.Length - 1);

                int spielZähler = 1;

                //Jedes verfügbare Spiel im Datagramm durchgehen, i+2, da jedes Spiel 2 Bytes braucht. Im ersten Byte der Wert für Link, das zweite Byte für den Wert rechts
                for (int i = 0; i < newData.Length; i += 2)
                {
                    var preGame = courtGames.FirstOrDefault(g => g.GameNumberOverAll == spielZähler - 1);
                    if (preGame != null)
                    {
                        preGame.MasterTurn.PointsTeamA = preGame.NetworkTurn.PointsTeamA;
                        preGame.MasterTurn.PointsTeamB = preGame.NetworkTurn.PointsTeamB;
                    }

                    var game = courtGames.FirstOrDefault(g => g.GameNumberOverAll == spielZähler);
                    if (game == null)
                        continue;

                    game.NetworkTurn.Reset();

                    if (tournament.IsDirectionOfCourtsFromRightToLeft)
                    {
                        if (game.TeamA.SteigendeSpielNummern.Contains(game.GameNumberOverAll))
                        {
                            // TeamA befindet sich bei diesem Spiel auf dieser Bahn rechts, 
                            // das nächste Spiel ist auf einer Bahn mit höherer oder gleicher Bahnnummer (1-> 2-> 3-> 4->...)
                            game.NetworkTurn.PointsTeamA = newData[i + 1];
                            game.NetworkTurn.PointsTeamB = newData[i];
                        }
                        else
                        {
                            // TeamA befindet sich bei diesem Spiel auf der Bahn links, das nächste Spiel ist auf einer Bahn mit niedrigerer Bahnnummer (5->4->3->2->1)
                            game.NetworkTurn.PointsTeamA = newData[i];
                            game.NetworkTurn.PointsTeamB = newData[i + 1];
                        }
                    }
                    else
                    {
                        if (game.TeamA.SteigendeSpielNummern.Contains(game.GameNumberOverAll))
                        {
                            // TeamA befindet sich in diesem Spiel auf dieser Bahn links, das nächste Spiel ist auf einer Bahn mit einer höheren Bahnnummer
                            game.NetworkTurn.PointsTeamA = newData[i];
                            game.NetworkTurn.PointsTeamB = newData[i + 1];
                        }
                        else
                        {
                            // TeamA befindet sich in diesem Spiel auf dieser Bahn rechts, das nächste Spiel ist auf einer Bahn mit einer niedrigeren Bahnnummer
                            game.NetworkTurn.PointsTeamA = newData[i + 1];
                            game.NetworkTurn.PointsTeamB = newData[i];
                        }
                    }

                    spielZähler++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                _CallBackAfterUpdateAction();
            }
        }

    }
}
