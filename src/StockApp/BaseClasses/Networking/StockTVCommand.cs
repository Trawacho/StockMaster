using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StockApp.BaseClasses
{

    public class StockTVCommand
    {
        readonly List<KeyValuePair<CommandTopics, string>> commandList;
        //readonly CommandTopics topic;
        //readonly string value;
        public readonly Action<byte[]> BackAction;

        //public bool IsGetCommand => IsGetSettingsCommand || IsGetResultsCommand;
        //public bool IsGetSettingsCommand => topic == CommandTopics.Get && value == GetCommandValues.Settings.ToString();
        //public bool IsGetResultsCommand => topic == CommandTopics.Get && value == GetCommandValues.Result.ToString();

        public string CommandString()
        {
            var commands = commandList.Select(x => String.Format("{0}={1}", x.Key, x.Value));
            var returnValue = String.Join(";", commands);
            return returnValue;
        }

        #region Konstruktor

        private StockTVCommand(CommandTopics topic, string value, Action<byte[]> action) : this(topic, value)
        {
            BackAction = action;
        }

        private StockTVCommand(CommandTopics topic, string value) : this()
        {
            commandList.Add(new KeyValuePair<CommandTopics, string>(topic, value));
        }

        private StockTVCommand()
        {
            commandList = new();
        }

        #endregion



        public static StockTVCommand SendSettingsCommand(StockTVSettings tVSettings)
        {
            StockTVCommand x = new();
            x.commandList.Add(new KeyValuePair<CommandTopics, string>(CommandTopics.GameModus, tVSettings.GameModus.ToString()));
            x.commandList.Add(new KeyValuePair<CommandTopics, string>(CommandTopics.ColorScheme, tVSettings.ColorScheme.ToString()));
            x.commandList.Add(new KeyValuePair<CommandTopics, string>(CommandTopics.TurnsPerGame, tVSettings.TurnsPerGame.ToString()));
            x.commandList.Add(new KeyValuePair<CommandTopics, string>(CommandTopics.PointsPerTurn, tVSettings.PointsPerTurn.ToString()));
            x.commandList.Add(new KeyValuePair<CommandTopics, string>(CommandTopics.NextLeft, tVSettings.NextLeft.ToString()));
            x.commandList.Add(new KeyValuePair<CommandTopics, string>(CommandTopics.Bahn, tVSettings.Bahn.ToString()));
            return x;
        }

        public static StockTVCommand SpielModusCommand(GameModis modus) { return new StockTVCommand(CommandTopics.GameModus, modus.ToString()); }
        public static StockTVCommand ColorModusCommand(ColorModis modus) { return new StockTVCommand(CommandTopics.ColorScheme, modus.ToString()); }
        public static StockTVCommand AnzahlKehrenProSpielCommand(int anzahl) { return new StockTVCommand(CommandTopics.TurnsPerGame, anzahl.ToString()); }
        public static StockTVCommand AnzahlPunkteProKehreCommand(int anzahl) { return new StockTVCommand(CommandTopics.PointsPerTurn, anzahl.ToString()); }
        public static StockTVCommand NextBahnLeftCommand(bool isLeft) { return new StockTVCommand(CommandTopics.NextLeft, isLeft.ToString()); }

        public static StockTVCommand ResetCommand() { return new StockTVCommand(CommandTopics.Reset, "true"); }
        public static StockTVCommand GetSettingsCommand(Action<byte[]> action) { return new StockTVCommand(CommandTopics.Get, GetCommandValues.Settings.ToString(), action); }
        public static StockTVCommand GetResultCommand(Action<byte[]> action) { return new StockTVCommand(CommandTopics.Get, GetCommandValues.Result.ToString(), action); }
        
        
        public static StockTVCommand SendBegegnungenCommand(IEnumerable<StockTVBegegnung> begegnungen)
        {
            string s = string.Empty;
            foreach (var b in begegnungen)
            {
                s += $"{b.SpielNummer}:{b.TeamNameA}:{b.TeamNameB};";
            }
            return new StockTVCommand(CommandTopics.SetBegegnungen, s);
        }
        
        public enum GameModis { Training, Turnier, BestOf, Ziel }
        public enum ColorModis { Normal, Dark }
        public enum CommandTopics { Bahn, ColorScheme, GameModus, PointsPerTurn, TurnsPerGame, NextLeft, Reset, Get, SetBegegnungen }
        public enum GetCommandValues { Settings, Result }

    }
}
