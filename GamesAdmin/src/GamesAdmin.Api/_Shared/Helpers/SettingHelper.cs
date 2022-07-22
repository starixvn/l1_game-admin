using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api._Shared.Helpers
{
    public static class SettingHelper
    {
        private static string BotRatioFormat = "#,##0.####";

        public static List<SettingChangedModel> GetCurrencyGroupGameChangedModel(CurrencyGroupGameEntity oldConfig, CurrencyGroupGame newConfig)
        {
            var changedSettings = new List<SettingChangedModel> {
                new SettingChangedModel("Enabled", oldConfig.Enabled.ToString(), newConfig.Enabled.ToString()),
                new SettingChangedModel("MinBet", oldConfig.MinBet.ToString(), newConfig.MinBet.ToString()),
                new SettingChangedModel("MaxBet", oldConfig.MaxBet.ToString(), newConfig.MaxBet.ToString()),
            };

            foreach (var c in newConfig.MaxBetPerChoices)
            {
                var oldValue = oldConfig.MaxBetPerChoices.ContainsKey(c.Key) ? oldConfig.MaxBetPerChoices[c.Key] : 0;
                var setting = new SettingChangedModel($"MaxBetPerChoices {c.Key}", oldValue.ToString(), c.Value.ToString());

                changedSettings.Add(setting);
            }

            if (newConfig.StakeLevels.Length == 0)
            {
                changedSettings.Add(new SettingChangedModel("StakeLevels", string.Join(", ", oldConfig.Chips), string.Join(", ", newConfig.Chips)));
                return changedSettings;
            }

            if (oldConfig.StakeLevels == null)
            {
                changedSettings.Add(new SettingChangedModel("StakeLevels", string.Empty, string.Join(", ", newConfig.StakeLevels.Select(s => s.Stake))));
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetBotRatiosChangedModel(double[] oldConfig, double[] newConfig)
        {
            var changedSettings = new List<SettingChangedModel>();

            for (var i = 0; i < newConfig.Length; i++)
            {
                var configPosition = i.ToGMT4();

                var setting = new SettingChangedModel(i.ToString(), oldConfig[configPosition].ToString(BotRatioFormat), newConfig[configPosition].ToString(BotRatioFormat));
                changedSettings.Add(setting);
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetBetLimitChangedModel(GameConfigEntity oldConfig, BetLimit newConfig)
        {
            var changedSettings = new List<SettingChangedModel>
            {
                new SettingChangedModel("Minbet", oldConfig.Minbet.ToString(), newConfig.MinBet.ToString()),
                new SettingChangedModel("Maxbet", oldConfig.Minbet.ToString(), newConfig.MaxBet.ToString()),
            };

            foreach (var c in newConfig.MaxBetChoices)
            {
                var oldText = oldConfig.MaxBetChoices == null ? string.Empty : oldConfig.MaxBetChoices[c.Key].ToString();
                var setting = new SettingChangedModel($"MaxBetPerChoices {c.Key}", oldText, c.Value.ToString());

                changedSettings.Add(setting);
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetSlotConfigChangedModel(
            GameConfigEntity oldConfig, bool botEnabled, SlotsConfig newConfig)
        {

            var changedSettings = new List<SettingChangedModel>
            {
                new SettingChangedModel("Bonus", GetValue(oldConfig.SlotsConfig.Bonus), GetValue(newConfig.Bonus)),
                new SettingChangedModel("BigWinRatio", oldConfig.SlotsConfig.BigWinRatio.ToString(), newConfig.BigWinRatio.ToString()),
                new SettingChangedModel("Mode", ((GameMode)oldConfig.SlotsConfig.Mode).ToString(), ((GameMode)newConfig.Mode).ToString()),
                new SettingChangedModel("Botenabled", oldConfig.Botenabled.ToString(), botEnabled.ToString()),
            };

            return changedSettings;
        }

        public static List<SettingChangedModel> GetGeneralConfigChangedModel(
            GameConfigEntity oldConfig,
            GameInfo gameInfo,
            BotSetting botSetting,
            EnableSetting enableSetting)
        {
            var changedSettings = new List<SettingChangedModel>
            {
                new SettingChangedModel("EnableFreeBet", oldConfig.EnableFreeBet.ToString(), gameInfo.EnableFreeBet.ToString()),
                new SettingChangedModel("DelayStartTime", oldConfig.DelayStartTime.ToString(), enableSetting.DelayStartTime.ToString()),
                new SettingChangedModel("Botenabled", oldConfig.Botenabled.ToString(), botSetting.BotEnabled.ToString()),
                new SettingChangedModel("BotMaxBet", oldConfig.BotMaxBet.ToString(), botSetting.BotMaxBet.ToString()),
                new SettingChangedModel("SmartBotEnabled", oldConfig.SmartBotEnabled.ToString(), botSetting.SmartBotEnabled.ToString()),
                new SettingChangedModel("SmartBotGroupRatio", oldConfig.SmartBotGroupRatio.ToString(), botSetting.SmartBotGroupRatio.ToString()),
                new SettingChangedModel("RandomMode", oldConfig.RandomMode.ToString(), gameInfo.RandomMode.ToString()),
            };

            return changedSettings;
        }

        public static List<SettingChangedModel> GetJackpotChangedModel(
            GameConfigEntity oldConfig, Jackpot jackpot, List<StakeLevel> stakeLevels)
        {
            var changedSettings = new List<SettingChangedModel>
            {
                new SettingChangedModel("ExcludeTestCurrency", oldConfig.SlotsConfig.Jackpot.ExcludeTestCurrency.ToString(), jackpot.ExcludeTestCurrency.ToString()),
                new SettingChangedModel("BotContributionRatio", oldConfig.SlotsConfig.Jackpot.BotContributionRatio.ToString(), jackpot.BotContributionRatio.ToString()),
                new SettingChangedModel("PlayerContributionRatio", oldConfig.SlotsConfig.Jackpot.PlayerContributionRatio.ToString(), jackpot.PlayerContributionRatio.ToString()),
                new SettingChangedModel("WinningRatio", oldConfig.SlotsConfig.Jackpot.WinningRatio.ToString(), jackpot.WinningRatio.ToString()),
            };

            for (var i = 0; i < stakeLevels.Count; i++)
            {

                var setting = new SettingChangedModel($"Stake level {i + 1}", GetValue(oldConfig.SlotsConfig.StakeLevels[i]), GetValue(stakeLevels[i]));

                changedSettings.Add(setting);
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetOddsChangedModel(Dictionary<string, double> oldConfig, Dictionary<string, double> newConfig)
        {
            var changedSettings = new List<SettingChangedModel>();

            foreach (var n in newConfig)
            {
                var oldText = oldConfig == null ? string.Empty : oldConfig[n.Key].ToString();
                var setting = new SettingChangedModel(n.Key, oldText, n.Value.ToString());
                changedSettings.Add(setting);
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetIconsChangedModel(Dictionary<string, Dictionary<string, double>> oldConfig, Dictionary<string, Dictionary<string, double>> newConfig)
        {
            var changedSettings = new List<SettingChangedModel>();

            foreach (var oldIcon in oldConfig)
            {
                var newIcon = newConfig[oldIcon.Key];

                for (var i = 0; i < oldIcon.Value.Count; i++)
                {
                    var oldOdds = oldIcon.Value.ElementAt(i);
                    var newOdds = newIcon.ElementAt(i);
                    var setting = new SettingChangedModel(
                        $"Icon {oldIcon.Key}",
                        $"Count {oldOdds.Key} - Odds {oldOdds.Value}",
                        $"Count {newOdds.Key} - Odds {newOdds.Value}");

                    changedSettings.Add(setting);
                }
            }

            return changedSettings;
        }

        private static string GetValue(Bonus bonus) => $"Min: {bonus.Min} - Max: {bonus.Max}";

        private static string GetValue(StakeLevel stakeLevel) => $"Initial Amount: {stakeLevel.Jackpot} - Bot Ratio: {stakeLevel.BotRatio}";
    }
}
