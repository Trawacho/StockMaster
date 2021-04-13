using System;
using System.Collections.Generic;
using System.Linq;

namespace StockApp.BaseClasses
{
    public delegate void NotifyResultInfoChangedHandler(object sender);

    public class StockTVResultMessage
    {
        public event NotifyResultInfoChangedHandler ResultInfoChanged;
        protected void RaiseResultInfoChanged()
        {
            var handler = ResultInfoChanged;
            handler?.Invoke(this);
        }

        public StockTVResultMessage()
        {
            this.Results = new List<GameResult>();
            this.LastHeartBeat = DateTime.Now;
        }
       

        public void SetValues(byte[] bytes)
        {
            byte b = Convert.ToByte(bytes[0]);
            if (Bahn != b) this.Bahn = b;

            var parts = bytes.Skip(1).Split(2);
            byte i = 1;
            this.Results.Clear();
            foreach (var item in parts)
            {
                this.Results.Add(new GameResult(item.First(), item.Last()) { GameNumber = i });
                i++;
            }
            RaiseResultInfoChanged();
        }

        public void SetHeartBeat()
        {
            LastHeartBeat = DateTime.Now;
        }

        public byte Bahn { get; private set; }
        public List<GameResult> Results { get; private set; }

        public DateTime LastHeartBeat { get; private set; }

        public class GameResult
        {
            public GameResult(byte a, byte b)
            {
                ValueA = Convert.ToByte(a);
                ValueB = Convert.ToByte(b);
            }
            public byte GameNumber { get; set; }
            public byte ValueA { get; private set; }
            public byte ValueB { get; private set; }
            public override string ToString()
            {
                return $"{ValueA}:{ValueB}";
            }
        }

        public override string ToString()
        {
            return $"Bahn: {Bahn} --> {string.Join("-", Results )}";
        }
    }
}
