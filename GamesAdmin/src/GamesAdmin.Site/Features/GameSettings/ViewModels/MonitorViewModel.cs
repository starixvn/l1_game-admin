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
        private static IList<string> gameEnabledHistory = new List<string>
        {
            GameType.BigSmallValue,
            GameType.BigSmallTurboValue,
            GameType.OddEvenValue,
            GameType.OddEvenTurboValue
        };

        private static Dictionary<string, string> gameHistoryUrl = new Dictionary<string, string>
        {
            { GameType.BigSmallValue, "bigsmall" },
            { GameType.BigSmallTurboValue, "bigsmallturbo" },
            { GameType.OddEvenValue, "oddeven" },
            { GameType.OddEvenTurboValue, "oddeventurbo" }
        };

        public MonitorItemViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? string.Empty : Enumeration.FromValue<GameType>(Name).DisplayName;

        public bool EnabledHistory => gameEnabledHistory.Contains(Name);

        public string GameHistoryUrl => gameHistoryUrl.ContainsKey(Name) ? gameHistoryUrl[Name] : string.Empty;
    }
}
