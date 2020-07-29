using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MonitorViewModel
    {
        public MonitorViewModel(IList<MonitorItemViewModel> gameList)
        {
            GameList = gameList;
        }

        public IList<MonitorItemViewModel> GameList { get; }
    }

    public class MonitorItemViewModel
    {
        private static Dictionary<string, string> gameHistoryUrl = new Dictionary<string, string>
        {
            { GameType.BigSmallValue, "bigsmall" },
            { GameType.BigSmallTurboValue, "bigsmallturbo" },
            { GameType.OddEvenValue, "oddeven" },
            { GameType.OddEvenTurboValue, "oddeventurbo" },
            { GameType.SeDieValue, "sedie" },
            { GameType.MiniSeDieValue, "minisedie" },
            { GameType.FishPrawnCrabValue, "fishprawncrab" },
            { GameType.DragonTigerValue, GameType.DragonTigerValue },
            { GameType.BaccaratValue, GameType.BaccaratValue },
            { GameType.KenoProVietValue, GameType.KenoProVietValue },
            { GameType.KenoMaxValue, GameType.KenoMaxValue },
            { GameType.KenoMax2Value, GameType.KenoMax2Value },
            { GameType.KenoMiniValue, GameType.KenoMiniValue },
            { GameType.KenoMini2Value, GameType.KenoMini2Value },
            { GameType.KenoEastValue, GameType.KenoEastValue },
            { GameType.KenoWestValue, GameType.KenoWestValue },
            { GameType.KenoSouthValue, GameType.KenoSouthValue },
            { GameType.KenoNorthValue, GameType.KenoNorthValue },
            { GameType.KenoProMaxValue, GameType.KenoProMaxValue }
        };

        public MonitorItemViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? string.Empty : Enumeration.FromValue<GameType>(Name).DisplayName;

        public bool EnabledHistory => gameHistoryUrl.ContainsKey(Name);

        public string GameHistoryUrl => gameHistoryUrl.ContainsKey(Name) ? gameHistoryUrl[Name] : string.Empty;
    }
}
