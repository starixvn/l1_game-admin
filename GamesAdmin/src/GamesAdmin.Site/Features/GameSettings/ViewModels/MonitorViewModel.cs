using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class MonitorViewModel
    {
        public MonitorViewModel(IList<MonitorItemViewModel> gameList, AdminMonitoringModel adminMonitoringModel)
        {
            GameList = gameList;
            JwtAdminUserToken = adminMonitoringModel.Token;
            AdminMonitoringUrl = adminMonitoringModel.Url;
        }

        public IList<MonitorItemViewModel> GameList { get; }

        public string AdminMonitoringUrl { get; }

        public string JwtAdminUserToken { get; }
    }

    public class MonitorItemViewModel
    {
        private static readonly string[] disableHistory = new string[]
        {
        };

        private static readonly HashSet<GameId> useHistoryUrlGameIds = new HashSet<GameId> {
            Core.Enumeration.GameId.BigSmall,
            Core.Enumeration.GameId.OddEven,
            Core.Enumeration.GameId.Blackjack,
            Core.Enumeration.GameId.BitcoinMaster,
            Core.Enumeration.GameId.CasinoHoldemPoker,
            Core.Enumeration.GameId.ChineseBlackjack,
            Core.Enumeration.GameId.FishPrawnCrab,
            Core.Enumeration.GameId.HiLo,
            Core.Enumeration.GameId.Pontoon,
            Core.Enumeration.GameId.SpecialSeDie,
            Core.Enumeration.GameId.TexasHoldemPoker,
            Core.Enumeration.GameId.BigSmallTurbo,
            Core.Enumeration.GameId.OddEvenTurbo,
            Core.Enumeration.GameId.SoloBlackjack
        };

        private static readonly string[] disableMonitoring = new[] {
            Game.LuckyWheelValue,
            Game.BlackJackValue
        };

        private static readonly string[] disableServiceStatus = new string[]
        {
        };

        public string Name { get; set; }

        public string Id { get; set; }
        public int GameId { get; set; }

        public string DBDisplayName { get; set; }

        public string DisplayName => Game.ContainsGame(Name)
                                        ? Game.FromGameName(Name).DisplayName
                                        : DBDisplayName;

        public GameType Type { get; set; }

        public bool EnabledMonitoring => !disableMonitoring.Contains(Name);

        public bool EnabledHistory => !disableHistory.Contains(Name);

        public bool EnabledServiceStatus => !disableServiceStatus.Contains(Name);

        public bool IsUseHistoryUrl => useHistoryUrlGameIds.Contains((GameId)GameId) || IsLuckyWheel;

        private bool IsLuckyWheel => Name == Game.LuckyWheelValue;

        public string GameHistoryUrl => string.IsNullOrWhiteSpace(DisplayName)
            ? Name.Replace(" ", string.Empty).Trim().ToLowerInvariant()
            : DisplayName.Replace(" ", string.Empty).Trim().Replace("'", string.Empty).ToLowerInvariant();

        public bool ApplyMultiplier { get; set; }
    }
}