using System.Collections.Generic;
using GamesAdmin.Core.Models;

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
        public StatusItemViewModel(GameConfig gameConfig)
        {
            GameConfig = gameConfig;
        }

        public GameConfig GameConfig { get; }

        public string Name => GameConfig.Info.Name;

        public string DisplayName => GameConfig.Info.DisplayName ?? GameConfig.Info.Name;

        public bool Enabled => GameConfig.EnableSetting.Enabled;

        public bool BotEnabled => GameConfig.BotSetting.BotEnabled;

        public string BotEnabledDisplay => BotEnabled ? "Yes" : "No";

        public string StatusDisplay => Enabled ? "Yes" : "No";

        public string DisabledMessage => GameConfig.EnableSetting.DisabledMessage;

        public double MinBet => GameConfig.BetLimit.MinBet;

        public double MaxBet => GameConfig.BetLimit.MaxBet;
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