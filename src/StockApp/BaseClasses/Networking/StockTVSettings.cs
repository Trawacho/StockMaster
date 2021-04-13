using System;
using System.Linq;
using System.Text;
using static StockApp.BaseClasses.StockTVCommand;

namespace StockApp.BaseClasses
{
    public class StockTVSettings
    {
        public StockTVSettings()
        {

        }
        public int Bahn { get; set; }
        public int PointsPerTurn { get; set; }
        public int TurnsPerGame { get; set; }
        public bool NextLeft { get; set; }
        public GameModis GameModus { get; set; }
        public ColorModis ColorScheme { get; set; }

        public void SetValues(byte[] array)
        {
            var parts = Encoding.UTF8.GetString(array).TrimEnd(';').Split(';');
            foreach (var part in parts)
            {
                var x = part.Split('=');
                var topic = x[0];
                var value = x[1];
                switch (topic)
                {
                    case nameof(Bahn):
                        Bahn = int.Parse(value);
                        break;
                    case nameof(PointsPerTurn):
                        PointsPerTurn = int.Parse(value);
                        break;
                    case nameof(TurnsPerGame):
                        TurnsPerGame = int.Parse(value);
                        break;
                    case nameof(NextLeft):
                        NextLeft = bool.Parse(value);
                        break;
                    case nameof(GameModus):
                        GameModus = (GameModis)Enum.Parse(typeof(GameModis), value);
                        break;
                    case nameof(ColorScheme):
                        ColorScheme = (ColorModis)Enum.Parse(typeof(ColorModis), value);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
