using System;

namespace StockApp.BaseClasses
{

    public class StockTVCommand
    {
        readonly CommandTopics topic;
        readonly string value;

        public bool IsGetCommand => IsGetSettingsCommand || IsGetResultsCommand;
        public bool IsGetSettingsCommand => topic == CommandTopics.Get && value == GetCommandValues.Settings.ToString();
        public bool IsGetResultsCommand => topic == CommandTopics.Get && value == GetCommandValues.Result.ToString();

        public override string ToString()
        {
            return $"{topic}={value}";
        }

        private StockTVCommand(CommandTopics topic, string value)
        {
            this.topic = topic;
            this.value = value;
        }

        public static StockTVCommand Get(string topic, string value)
        {

            if (Enum.TryParse<CommandTopics>(topic, out CommandTopics t))
            {
                return new StockTVCommand(t, value);
            }

            return null;
        }

        public static StockTVCommand SpielModusCommand(GameModis modus) { return new StockTVCommand(CommandTopics.GameModus, modus.ToString()); }
        public static StockTVCommand ColorModusCommand(ColorModis modus) { return new StockTVCommand(CommandTopics.ColorScheme, modus.ToString()); }
        public static StockTVCommand AnzahlKehrenProSpielCommand(int anzahl) { return new StockTVCommand(CommandTopics.TurnsPerGame, anzahl.ToString()); }
        public static StockTVCommand AnzahlPunkteProKehreCommand(int anzahl) { return new StockTVCommand(CommandTopics.PointsPerTurn, anzahl.ToString()); }
        public static StockTVCommand NextBahnLeftCommand(bool isLeft) { return new StockTVCommand(CommandTopics.NextLeft, isLeft.ToString()); }

        public static StockTVCommand ResetCommand() { return new StockTVCommand(CommandTopics.Reset, "true"); }
        public static StockTVCommand GetSettingsCommand() { return new StockTVCommand(CommandTopics.Get, GetCommandValues.Settings.ToString()); }
        public static StockTVCommand GetResultCommand() { return new StockTVCommand(CommandTopics.Get, GetCommandValues.Result.ToString()); }
        public enum GameModis { Training, Turnier, BestOf, Ziel }
        public enum ColorModis { Normal, Dark }
        public enum CommandTopics { Bahn, ColorScheme, GameModus, PointsPerTurn, TurnsPerGame, NextLeft, Reset, Get }
        public enum GetCommandValues { Settings, Result }
       
    }
}
