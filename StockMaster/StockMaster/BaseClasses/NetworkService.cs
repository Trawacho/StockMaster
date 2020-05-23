using StockMaster.ViewModels;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace StockMaster.BaseClasses
{
    internal class NetworkService
    {
        private UdpClient udpClient;
        private UdpState state;
        private readonly Tournament tournament;
        private readonly Action _CallBackAfterUpdateAction;


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
        }

        public void Stop()
        {
            udpClient.Dispose();
            udpClient = null;
            state.udpClient = null;
            state = null;
        }

        public bool IsRunning()
        {
            return (udpClient != null);
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
                        tournament.DeleteAllTurnsInEveryGame();
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
            /* 0      1              2..
             * Bahn - AnzahlKehren - KK.......   KK........  KK......
             * 1       1             2*anzKehren 2*anzKehren max 2*anzKehren 
             *
             * Aufbau eines Datagramms: 
             * 1 Im ersten Byte steht die Bahnnummer
             * 2 Im zweiten Byten steht die Länge eines Spiels (Anzahl der Kehren)
             * 3 In jedem weiteren Byten kommen die laufenden Kehren
             *   Jede Kehre besteht aus zwei Byte, davon
             *      das erste Byte hat Mannschaft links
             *      das zweite Byte hat Mannschaft rechts
             * Durch das zweite Byten im Datagramm (Anzahl der Kehren) kann man die einzelnen Spiele trennen  
             * Das letzte Spiel kann kürzer sein. Es sind evtl. nicht alle Kehren im Datagramm
             * 
             */

            if (data == null)
                return;

            byte bahnNumber = data[0];
            var courtGames = tournament.GetGamesOfCourt(bahnNumber);

            byte turnLength = data[1];

            //Die ersten beiden Byte aus dem Array werden nicht mehr benötigt, Daten in ein neues Array kopieren
            byte[] newData = new byte[data.Length - 2];
            Array.Copy(data, 2, newData, 0, data.Length - 2);

            //Jede verfügbare Kehre im Datagramm durchgehen, i+2, da immer 2 Bytes pro Kehre
            // Zähler für die Spiele mitzählen
            int spielZähler = 1;
            int kehrenZähler = 1;
            var game = courtGames.First(g => g.GameNumber == spielZähler);
            game.Turns.Clear();

            for (int i = 0; i < newData.Length; i += 2)
            {
                if (kehrenZähler > turnLength)
                {
                    kehrenZähler = 1;
                    spielZähler++;
                    game = courtGames.First(g => g.GameNumber == spielZähler);
                    game.Turns.Clear();
                }

                game.Turns.Push(new Turn(kehrenZähler)
                {
                    PointsTeamB = newData[i],
                    PointsTeamA = newData[i + 1]
                });

                kehrenZähler++;

            }
         
            _CallBackAfterUpdateAction();
        }

        


    }
}
