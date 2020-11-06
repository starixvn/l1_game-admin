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
        public StatusItemViewModel(string name, bool enabled, string disabledMessage)
        {
            Name = name;
            Enabled = enabled;
            DisabledMessage = disabledMessage;
        }

        public string Name { get; }

        public string DisplayName =>  string.IsNullOrWhiteSpace(Name) || !Enumeration.Contains<Game>(Name)
            ? Name
            : Enumeration.FromValue<Game>(Name).DisplayName;

        public bool Enabled { get; }

        public bool BotEnabled { get; set; }

        public string BotEnabledDisplay => BotEnabled ? "Yes" : "No";

        public string StatusDisplay => Enabled ? "Yes" : "No";

        public string DisabledMessage { get; }

        public double MinBet { get; set; }

        public double MaxBet { get; set; }
    }

    public class GameDisableMessage
    {
        public string Name { get; set; }

        public string DisabledMessage { get; set; }
    }

    public class GameServerToken
    {
        public GameServerToken(string token, string env, string redirectUrl)
        {
            Token = token;
            Env = env;
            RedirectUrl = redirectUrl;
        }

        public string Token { get; }

        public string Env { get; }

        public string RedirectUrl { get; }
    }
}
