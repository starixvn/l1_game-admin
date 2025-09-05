using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Core.Models.Partners;

namespace GamesAdmin.Site.Features.GameSettings.ViewModels
{
    public class StatusViewModel
    {
        public StatusViewModel(IList<StatusItemViewModel> gameList,
            IList<Partner> partners = null,
            IList<CurrencyGroupModel> currencyGroups = null,
            IList<CurrencyGroupGame> currencyGroupGames = null)
        {
            GameList = gameList;
            Partners = partners;
            CurrencyGroups = currencyGroups;
            CurrencyGroupGames = currencyGroupGames;
        }

        public IList<StatusItemViewModel> GameList { get; }

        public IList<Partner> Partners { get; }

        public IList<CurrencyGroupModel> CurrencyGroups { get; }

        public IList<CurrencyGroupGame> CurrencyGroupGames { get; }

        public string GetCurrencyGroupOfferGame(GameConfig gameConfig, string operatorId)
        {
            var offeredGroups = new List<string>();
            var currencyGroups = CurrencyGroups.Where(group => group.PartnerId == operatorId);
            var currencyGroupGames = CurrencyGroupGames?.Where(groupGame => groupGame.GameId == gameConfig.GameId);

            foreach (var currencyGroup in currencyGroups)
            {
                var setting = currencyGroupGames?.SingleOrDefault(groupGame => groupGame.GroupId == currencyGroup.Id);
                if (setting != null && setting.Enabled)
                {
                    var groupInfo = $"<b>{currencyGroup.Name}</b>";

                    switch (gameConfig.Info.Type)
                    {
                        case GameType.CascadingSlots:
                            var minBetLevel = gameConfig.CascadingSlotsConfig.BetLevels.Min();
                            var maxBetLevel = gameConfig.CascadingSlotsConfig.BetLevels.Max();
                            var minCoin = setting.CoinValues.Min();
                            var maxCoin = setting.CoinValues.Max();

                            groupInfo += ": " + string.Join(" - ", new double[] { gameConfig.CascadingSlotsConfig.BetMultiplier * minCoin * minBetLevel, gameConfig.CascadingSlotsConfig.BetMultiplier * maxCoin * maxBetLevel });
                            break;
                        case GameType.Slots:
                            groupInfo += ": " + (setting.StakeLevels.Length > 0 ? string.Join(" - ", setting.StakeLevels.Select(stake => stake.Stake).ToArray()) : string.Join(" - ", setting.BetLevels));
                            break;
                        default:
                            groupInfo += ": " + string.Join(" - ", new double[] { setting.MinBet, setting.MaxBet });
                            break;
                    }


                    offeredGroups.Add(groupInfo);
                }
            }

            return string.Join("<br/>", offeredGroups);
        }
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

        public int GameId => Game.ContainsGame(Name) ? (int)Game.FromGameName(Name).GameId : GameConfig.Info.GameId;

        private string ParseGameConfigName()
            => Game.ContainsGame(GameConfig.Info.Name) ? Game.FromGameName(GameConfig.Info.Name).DisplayName : GameConfig.Info.DisplayName;

        public bool Enabled => GameConfig.EnableSetting.Enabled;

        public bool BotEnabled => GameConfig.BotSetting.BotEnabled;

        public string DisabledMessage => GameConfig.EnableSetting.DisabledMessage;
    }

    public class GameDisableMessage
    {
        public string Name { get; set; }

        public string DisabledMessage { get; set; }
    }

    public class GameServerToken
    {
        public GameServerToken(string token, string gameName)
        {
            Token = token;
            GameName = gameName;
        }

        public string Token { get; }

        public string GameName { get; }
    }
}