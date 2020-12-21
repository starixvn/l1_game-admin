using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
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

        public string DisplayName => ParseGameConfigName();

        private string ParseGameConfigName() 
            => Enumeration.Contains<Game>(GameConfig.Info.Name) ? Enumeration.FromValue<Game>(GameConfig.Info.Name).DisplayName : GameConfig.Info.Name;

        public bool Enabled => GameConfig.EnableSetting.Enabled;

        public bool BotEnabled => GameConfig.BotSetting.BotEnabled;

        public string DisabledMessage => GameConfig.EnableSetting.DisabledMessage;

        public string MinMaxBet => GameConfig.Info.Type == GameType.Slots
            ? string.Join(" - ", GameConfig.SlotsConfig.StakeLevels.Select(x => x.Stake).ToArray())
            : $"{GameConfig.BetLimit.MinBet} - {GameConfig.BetLimit.MaxBet}";
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