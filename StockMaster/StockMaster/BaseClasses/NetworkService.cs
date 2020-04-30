using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.SqlServer.Server;
using System.Resources;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    internal class NetworkService : IDisposable
    {
        public static UdpClient udpClient = new UdpClient(Settings.Instanze.BroadcastPort);
        public static UdpState state;//= new UdpState();
        public class UdpState
        {
            public UdpClient udpClient;
            public IPEndPoint ipEndPoint;
            public IAsyncResult result;
            public bool cancel;
        }

        private readonly Tournament tournament;

        public NetworkService(Tournament tournament)
        {
            this.tournament = tournament;
            udpClient.Client.ReceiveTimeout = 1000;
        }

        public void ReceiveBroadcast()
        {
            //SendOnePacket();
            //Thread.Sleep(1000);
            state = new UdpState()
            {
                udpClient = udpClient,
                ipEndPoint = new IPEndPoint(IPAddress.Any, Settings.Instanze.BroadcastPort),
                cancel = false
            };

            state.result = state.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), state);
        }

        void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                UdpClient u = ((UdpState)ar.AsyncState).udpClient;
                IPEndPoint e = ((UdpState)ar.AsyncState).ipEndPoint;
                IAsyncResult r = ((UdpState)ar.AsyncState).result;
                bool c = ((UdpState)ar.AsyncState).cancel;

                byte[] receiveBytes = u.EndReceive(ar, ref e);
                if (receiveBytes.Length > 1)
                {
                    DeSerialize(DeCompress(receiveBytes));
                }
                else
                {
                    if (receiveBytes[0] == byte.MaxValue)
                    {
                        Parallel.ForEach(tournament.Games, (g) =>
                        {
                            g.Turns.Clear();
                        });
                       
                    }
                }

                if (!c)
                    r = u.BeginReceive(new AsyncCallback(ReceiveCallback), state);
            }
            catch (ObjectDisposedException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        static byte[] DeCompress(byte[] data)
        {
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

            byte bahnNumber = data[0];
           

            var courtGames = tournament.Games.Where(g => g.CourtNumber == bahnNumber).Distinct();


            byte turnLength = data[1];

            // turnier.Games.RemoveAll(g => g.BahnNumber == bahnNumber);

            //Die ersten beiden Byte aus dem Array werden nicht mehr benötigt, Daten in ein neues Array kopieren
            byte[] newData = new byte[data.Length - 2];
            Array.Copy(data, 2, newData, 0, data.Length - 2);

            //var g = new Game(bahnNumber);

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
            tournament.RaisePropertyChanged(nameof(Tournament.Ergebnisliste));

        }

        public void Dispose()
        {
            state.cancel = true;
            state.udpClient.Close();
            state.udpClient.Dispose();
            udpClient.Close();
            udpClient.Dispose();
        }

        public void SendOnePacket()
        {

            using (var c = new UdpClient())
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.100.255"), 4711);
                byte[] bytes = Encoding.ASCII.GetBytes("STOP");
                c.Send(bytes, bytes.Length, ip);
                c.Close();
            }

        }
    }
}
