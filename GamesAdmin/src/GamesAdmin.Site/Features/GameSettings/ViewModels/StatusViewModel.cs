using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class StatusViewModel
    {
        public StatusViewModel(IList<StatusItemViewModel> gameList) 
        {
            GameList = gameList;
        }

        public IList<StatusItemViewModel> GameList { get; }
    }

    public class StatusItemViewModel 
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

        public StatusItemViewModel(string name, bool enabled, string disabledMessage) 
        {
            Name = name;
            Enabled = enabled;
            DisabledMessage = disabledMessage;
        }

        public string Name { get; }

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? string.Empty : Enumeration.FromValue<GameType>(Name).DisplayName;

        public bool Enabled { get; }

        public bool BotEnabled { get; set; }

        public string BotEnabledDisplay => BotEnabled ? "Yes" : "No";

        public string StatusDisplay => Enabled ? "Yes" : "No";

        public string DisabledMessage { get; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }

        public bool EnabledHistory => gameEnabledHistory.Contains(Name);

        public string GameHistoryUrl => gameHistoryUrl.ContainsKey(Name) ? gameHistoryUrl[Name] : string.Empty;
    }

    public class GameDisableMessage 
    {
        public string Name { get; set; }

        public string DisabledMessage { get; set; }
    }

    public class JWTFromGameServer
    {
        public string Token { get; set; }

        public string GameServerEndpoint { get; set; }
    }
}
