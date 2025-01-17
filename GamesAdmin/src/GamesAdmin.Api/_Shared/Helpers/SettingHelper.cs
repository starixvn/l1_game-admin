using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Extensions;
using GamesAdmin.Core.IdentityServer;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Core.Models.Promotion;
using GamesAdmin.Database.Entities;
using Newtonsoft.Json;
using Refit;

namespace GamesAdmin.Api._Shared.Helpers
{
    public static class SettingHelper
    {
        private static string BotRatioFormat = "#,##0.####";

        public static List<SettingChangedModel> GetSymbolWeightChangedModel(Dictionary<string, int[][]> oldSymbolWeight, SymbolWeight newConfig)
        {
            var weight = oldSymbolWeight?.ContainsKey(newConfig.Mode.ToString()) == true ? oldSymbolWeight[newConfig.Mode.ToString()] : null;
            var changedSettings = new List<SettingChangedModel>();

            for (var i = 0; i < newConfig.Weight.Length; i++)
            {
                var oldText = weight == null ? string.Empty : ConvertMatrixToString(weight[i]);
                var newText = ConvertMatrixToString(newConfig.Weight[i]);
                var setting = new SettingChangedModel($"Weight Row {i + 1}", oldText, newText);
                changedSettings.Add(setting);
            }

            return changedSettings;
        }

        private static string ConvertMatrixToString(int[] row)
        {
            return string.Join(", ", row);
        }

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

            if (newConfig.BetLevels.Length == 0)
            {
                changedSettings.Add(new SettingChangedModel("StakeLevels", string.Join(", ", oldConfig.Chips), string.Join(", ", newConfig.Chips)));
                return changedSettings;
            }

            if (oldConfig.BetLevels == null)
            {
                changedSettings.Add(new SettingChangedModel("StakeLevels", string.Empty, string.Join(", ", newConfig.BetLevels)));
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

            if (newConfig.MaxBetChoices != null)
            {
                foreach (var c in newConfig.MaxBetChoices)
                {
                    var oldText = oldConfig.MaxBetChoices == null ? string.Empty : oldConfig.MaxBetChoices[c.Key].ToString();
                    var setting = new SettingChangedModel($"MaxBetPerChoices {c.Key}", oldText, c.Value.ToString());

                    changedSettings.Add(setting);
                }
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
            EnableSetting enableSetting,
            OddsMultipliersConfig oddsMultipliersConfig,
            TournamentConfig tournamentConfig,
            VideoStreamConfig videoStreamConfig)
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
                new SettingChangedModel("MaxWinPerRound", oldConfig.MaxWinPerRound.ToString(), gameInfo.MaxWinPerRound.ToString()),
            };

            if (oldConfig.ApplyMultiplier && oddsMultipliersConfig != null)
            {
                changedSettings.Add(new SettingChangedModel("OddsMultipliers", JsonConvert.SerializeObject(oldConfig.OddsMultipliers), JsonConvert.SerializeObject(oddsMultipliersConfig)));
            }

            if (oldConfig.HaveTournament && tournamentConfig != null)
            {
                changedSettings.Add(new SettingChangedModel("TournamentConfig", JsonConvert.SerializeObject(oldConfig.TournamentConfig), JsonConvert.SerializeObject(tournamentConfig)));
            }

            if (oldConfig.VideoStreamConfig != null && videoStreamConfig != null)
            {
                changedSettings.Add(new SettingChangedModel("VideoStreamConfig", JsonConvert.SerializeObject(oldConfig.VideoStreamConfig), JsonConvert.SerializeObject(videoStreamConfig)));
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetJackpotChangedModel(
            GameConfigEntity oldConfig, Jackpot jackpot)
        {
            var changedSettings = new List<SettingChangedModel>
            {
                new SettingChangedModel("ExcludeTestCurrency", oldConfig.SlotsConfig.Jackpot.ExcludeTestCurrency.ToString(), jackpot.ExcludeTestCurrency.ToString()),
                new SettingChangedModel("Pool", oldConfig.SlotsConfig.Jackpot.Pools?.ToString(), jackpot.Pools?.ToString()),
                new SettingChangedModel("BotContributionRatio", oldConfig.SlotsConfig.Jackpot.BotContributionRatio.ToString(), jackpot.BotContributionRatio.ToString()),
                new SettingChangedModel("PlayerContributionRatio", oldConfig.SlotsConfig.Jackpot.PlayerContributionRatio.ToString(), jackpot.PlayerContributionRatio.ToString()),
            };

            return changedSettings;
        }

        public static List<SettingChangedModel> GetBotConfigChangedModel(
            GameConfigEntity oldConfig, Jackpot jackpot, double[] betLevels)
        {
            var changedSettings = new List<SettingChangedModel>
            {
                new SettingChangedModel("BetLevels", oldConfig.SlotsConfig.BetLevels.ToString(), betLevels.ToString()),
                new SettingChangedModel("Pool", oldConfig.SlotsConfig.Jackpot.Pools?.ToString(), jackpot.Pools?.ToString())
            };

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

        public static List<SettingChangedModel> GetMarginSettingsChangedModel(IEnumerable<Dictionary<string, double>> oldSettings, IEnumerable<Dictionary<string, double>> newSettings)
        {
            var changedSettings = new List<SettingChangedModel>();
            var newSettingList = newSettings.ToList();
            var oldSettingList = oldSettings?.ToList();

            for (var i = 0; i < newSettingList.Count; i++)
            {
                foreach (var (key, value) in newSettingList[i])
                {
                    var oldText = oldSettingList?.Count <= i || oldSettingList?[i] == null
                        ? "0"
                        : oldSettingList[i][key].ToString();
                    var setting = new SettingChangedModel($"range {i}-{key}", oldText, value.ToString());
                    changedSettings.Add(setting);
                }
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

        public static List<SettingChangedModel> GetCascadingSlotOddsChangedModel(CascadingSlotsConfig oldConfig, CascadingSlotsConfig newConfig)
        {
            var oldOddsByCount = oldConfig.OddsByCounts;
            var newOddsByCount = newConfig.OddsByCounts;
            var oldInitialFreeSpinOdds = oldConfig.FreeSpins.InitialOdds;
            var newInitialFreespinOdds = newConfig.FreeSpins.InitialOdds;

            var changedSettings = new List<SettingChangedModel>
            {
               new SettingChangedModel("Additional Free Spins Bonus", oldConfig.FreeSpins.AdditionalOdds.Count.ToString(), newConfig.FreeSpins.AdditionalOdds.Count.ToString()),
            };

            for (int i = 0; i < newInitialFreespinOdds.Count; i++)
            {
                var oldSettingsString = $"[{oldInitialFreeSpinOdds[i].Count}] : {oldInitialFreeSpinOdds[i].Odds}";
                var newSettingsString = $"[{newInitialFreespinOdds[i].Count}] : {newInitialFreespinOdds[i].Odds}";
                var settingChanged = new SettingChangedModel("Initial FreeSpin Odds", oldSettingsString, newSettingsString);

                changedSettings.Add(settingChanged);
            }

            foreach (var iconSettings in newOddsByCount)
            {
                var oldValue = GetOddsByCountConfigString(iconSettings.Value);
                var newValue = GetOddsByCountConfigString(oldOddsByCount[iconSettings.Key]);
                var settingChanged = new SettingChangedModel($"Odds Icon {iconSettings.Key + 1}", oldValue, newValue);

                changedSettings.Add(settingChanged);
            }

            return changedSettings;
        }

        public static List<SettingChangedModel> GetCascadingSlotConfigsChangeModel(GameConfigEntity gameConfig, bool newBotEnabled, CascadingSlotsConfig newConfig)
        {
            var oldConfig = gameConfig.CascadingSlotsConfig;

            var settingChanged = new List<SettingChangedModel>
            {
                new SettingChangedModel("Bot Enable", gameConfig.Botenabled.ToString(), newBotEnabled.ToString()),
                new SettingChangedModel("Multiplier Bonus", newConfig.MultiplierBonus.ToString(), oldConfig.MultiplierBonus.ToString()),
                new SettingChangedModel("Big Win Ratio", newConfig.BigWinRatio.ToString(), oldConfig.BigWinRatio.ToString()),
                new SettingChangedModel("Mode", newConfig.Mode.ToString(), oldConfig.Mode.ToString()),
                new SettingChangedModel("Bet Multiplier", newConfig.BetMultiplier.ToString(), oldConfig.BetMultiplier.ToString()),
                new SettingChangedModel("Bet Level", newConfig.BetLevels.ToString(), oldConfig.BetLevels.ToString()),
            };

            return settingChanged;
        }

        public static List<SettingChangedModel> GetSlotRNGConfigsChangeModel(
             GameConfigEntity oldConfig,
            GameInfo gameInfo,
            BotSetting botSetting,
            EnableSetting enableSetting,
            SlotRNGConfig slotRNGConfig)
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
                new SettingChangedModel("Mode", oldConfig.SlotRNGConfig.Mode.ToString(), slotRNGConfig.Mode.ToString()),
                new SettingChangedModel("Lucky", $"({oldConfig.SlotRNGConfig.LuckySpinFrom}-{oldConfig.SlotRNGConfig.LuckySpinTo})", $"({slotRNGConfig.LuckySpinFrom}-{slotRNGConfig.LuckySpinTo})"),
            };

            return changedSettings;
        }

        public static List<SettingChangedModel> GetCrashConfigChangeModel(
           GameConfigEntity oldConfig,
           GameInfo gameInfo,
           BotSetting botSetting,
           EnableSetting enableSetting,
           CrashConfig crashConfig)
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
                new SettingChangedModel("Max Multiplir", oldConfig.CrashConfig.MaxMultiplier.ToString(), crashConfig.MaxMultiplier.ToString()),
                new SettingChangedModel("Max Win per Round", oldConfig.CrashConfig.MaxWinPerRound.ToString(), crashConfig.MaxWinPerRound.ToString()),
                new SettingChangedModel("Crash at Base (%)", oldConfig.CrashConfig.CrashAtBase.ToString(), crashConfig.CrashAtBase.ToString()),
            };

            return changedSettings;
        }

        private static string GetOddsByCountConfigString(IList<OddsByCount> oddsByCount)
        {
            string configString = "";

            for (int i = 0; i < oddsByCount.Count; i++)
            {
                configString += $"[{oddsByCount[i].From} - {oddsByCount[i].To}] : {oddsByCount[i].Odds}; ";
            }

            return configString;
        }

        private static string GetValue(Bonus bonus) => $"Min: {bonus.Min} - Max: {bonus.Max}";

        private static string GetValue(StakeLevel stakeLevel) => $"Initial Amount: {stakeLevel.Jackpot} - Bot Ratio: {stakeLevel.BotRatio}";

        public static List<SettingChangedModel> GetPromotionTournamentChangeModel(PromotionConfigModel oldConfig, PromotionConfigModel newConfig, string type)
        {
            if (type.Equals(PromotionTournamentTypeUpdate.OperatorSetting))
            {
                var oldListOperators = FormatOperators(oldConfig.Operators);
                var newListOperators = FormatOperators(newConfig.Operators);
                return new List<SettingChangedModel>
                {
                    new SettingChangedModel("Operators", oldListOperators, newListOperators),
                };
            }
            else if (type.Equals(PromotionTournamentTypeUpdate.EditTournament))
            {
                var oldListWinningPrizes = FormatWinningPrizes(oldConfig.WinningPrizes);
                var newListWinningPrizes = FormatWinningPrizes(newConfig.WinningPrizes);
                return new List<SettingChangedModel>
                {
                    new SettingChangedModel("Promotion Name", oldConfig.Name, newConfig.Name),
                    new SettingChangedModel("Start Date Time", oldConfig.StartDateTime.ToString(), newConfig.StartDateTime.ToString()),
                    new SettingChangedModel("End Date Time", oldConfig.EndDateTime.ToString(), newConfig.EndDateTime.ToString()),
                    new SettingChangedModel("Winning Prizes", oldListWinningPrizes, newListWinningPrizes),
                };
            }

            return new List<SettingChangedModel>();
        }

        public static string FormatOperators(List<OperatorModel> operators)
        {
            return string.Join("\r\n", operators.Select(o =>
            {
                var currencyGroupsFormatted = string.Join("\r\n\t", o.CurrencyGroups.Select(cg =>
                    $"+ Currency Group: {cg.CurrencyName}, Slot Game Min Bet: {cg.SlotGameMinBet}, Other Game Min Bet: {cg.OtherGameMinBet}, Enabled: {cg.Enabled}"));

                return $"- {o.OperatorName}\r\n\t+ Included Sites: {o.IncludedSiteNames}\r\n\t{currencyGroupsFormatted}";
            }));
        }

        public static string FormatWinningPrizes(List<WinningPrizeModel> winningPrizes)
        {
            return string.Join("\r\n\t", winningPrizes.Select(wp =>
                $"- Ranking {wp.Ranking}, Value: {wp.Value}"));
        }

        public static RefitSettings GetRefitSettings()
        {
            return new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(),
                AuthorizationHeaderValueGetter = IdentityServerManager.GetAccessToken,
            };
        }
    }
}