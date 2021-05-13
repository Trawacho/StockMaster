using System;
using System.Text;

namespace StockApp.BaseClasses
{
    public class StockTVSettings : IEquatable<StockTVSettings>
    {
        public static StockTVSettings GetDefault(GameModis modus)
        {
            var s = new StockTVSettings()
            {
                Bahn = -1,
                NextBahnModus = NextBahnModis.Left,
                ColorModus = ColorModis.Normal,
                GameModus = modus
            };
            switch (modus)
            {
                case GameModis.Training:
                    s.PointsPerTurn = 30;
                    s.TurnsPerGame = 30;
                    break;
                case GameModis.Turnier:
                case GameModis.BestOf:
                    s.PointsPerTurn = 15;
                    s.TurnsPerGame = 6;
                    break;
                case GameModis.Ziel:
                    break;
                default:
                    break;
            }

            return s;
        }

        private StockTVSettings()
        {

        }

        public StockTVSettings(byte[] array) : this()
        {
            SetValues(array);
        }

        public StockTVSettings(string valueString) : this()
        {
            SetValues(valueString);
        }

        public int Bahn { get; set; }
        public int PointsPerTurn { get; set; }
        public int TurnsPerGame { get; set; }

        
        public NextBahnModis NextBahnModus { get; set; }

        public GameModis GameModus { get; set; }
        public ColorModis ColorModus { get; set; }

        public bool Equals(StockTVSettings other)
        {
            return Bahn == other.Bahn
                && PointsPerTurn == other.PointsPerTurn
                && TurnsPerGame == other.TurnsPerGame
                && NextBahnModus == other.NextBahnModus
                && GameModus == other.GameModus
                && ColorModus == other.ColorModus;
        }

        public void SetValues(string valueString)
        {
            var parts = valueString.TrimEnd(';').Split(';');

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
                    case nameof(NextBahnModus):
                        NextBahnModus = (NextBahnModis)Enum.Parse(typeof(NextBahnModis), value);
                        break;
                    case nameof(GameModus):
                        GameModus = (GameModis)Enum.Parse(typeof(GameModis), value);
                        break;
                    case nameof(ColorModus):
                        ColorModus = (ColorModis)Enum.Parse(typeof(ColorModis), value);
                        break;
                    default:
                        break;
                }
            }
        }

        public void SetValues(byte[] array)
        {
            SetValues(Encoding.UTF8.GetString(array));
        }

        public override string ToString()
        {
            return $"Bahn:{Bahn} | GameModus:{GameModus} | ColorModus:{ColorModus} | PointsPerTurn:{PointsPerTurn} | TurnsPerGame:{TurnsPerGame} | NextBahn:{NextBahnModus}  ";
        }
    }
}
