using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.Notifications;
using GamesAdmin.Api.Notifications.Requests;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.Games
{
    public interface IGameSettingsService
    {
        Task<IEnumerable<GameConfig>> Get();

        Task<bool> Create(GameConfig game);

        Task<GameConfig> GetByName(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisabledMessage(string name, Dictionary<string, string> messages, bool isApplyAll = false);

        Task<bool> UpdateBetLimit(string name, BetLimit betLimit);

        Task<bool> UpdateOdds(string name, Dictionary<string, double> odds);

        Task<bool> UpdateBotRatios(string name, double[] ratios);

        Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds);

        Task<bool> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig);

        Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels);

        Task<bool> UpdateSlotConfig(string name, bool botEnabled, SlotsConfig slotsConfig);

        Task<bool> UpdateCascadingSlotsConfig(string name, bool botEnabled, CascadingSlotsConfig cascadingSlotsConfig);

        Task<bool> UpdateGeneralConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting);

        Task<Dictionary<string, string>> GetDisabledMessage(string name);

        Task<bool> UpdateSlotRNGConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, SlotRNGConfig slotRNGConfig);
    }

    public class GameSettingsService : IGameSettingsService
    {
        private const byte HoursOfDay = 24;

        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<GameConfigEntity> repository;
        private readonly INotificationService notificationService;

        public GameSettingsService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<GameConfigEntity> repository,
            INotificationService notificationService
            )
        {
            this.repository = repository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
            this.notificationService = notificationService;
        }

        public async Task<IEnumerable<GameConfig>> Get()
        {
            try
            {
                var gameEntities = await Task.Run(() => repository.AsQueryable().ToList());

                return gameEntities.Select(entity => mapper.Map<GameConfig>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<GameConfig>();
            }
        }

        public async Task<bool> Create(GameConfig game)
        {
            try
            {
                var gameId = game.Info.Type == GameType.Slots
                    ? game.Info.GameId
                    : (int)Enumeration.FromValue<Game>(game.Info.Name).GameId;

                var entity = new GameConfigEntity
                {
                    Name = game.Info.Name,
                    Type = (byte)game.Info.Type,
                    EnableFreeBet = game.Info.EnableFreeBet,
                    Enabled = game.EnableSetting.Enabled,
                    DelayStartTime = game.EnableSetting.DelayStartTime,
                    DisabledMessage = game.EnableSetting.DisabledMessage,
                    Maxbet = game.BetLimit.MaxBet,
                    Minbet = game.BetLimit.MinBet,
                    Botenabled = game.BotSetting.BotEnabled,
                    BotMaxBet = game.BotSetting.BotMaxBet,
                    HoursMaxBot = game.BotSetting.HoursMaxBot,
                    SlotsConfig = game.SlotsConfig,
                    CascadingSlotsConfig = game.CascadingSlotsConfig,
                    SlotRNGConfig = game.SlotRNGConfig,
                    SiteExclusions = new string[] { },
                    Time = DateTime.Now,
                    GameId = gameId,
                    DisplayName = game.Info.DisplayName,
                };

                await repository.InsertOneAsync(entity);

                var messages = new Dictionary<string, string> { [Language.DefaultLanguageValue] = game.EnableSetting.DisabledMessage };
                await notificationService.AddDisableMessage(game.Info.Name, messages);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }

            return true;
        }

        public async Task<GameConfig> GetByName(string name)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);

            return gameEntity != null ? mapper.Map<GameConfig>(gameEntity) : default;
        }

        public async Task<bool> UpdateStatus(string[] names, bool enabled)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update.Set(field => field.Enabled, enabled);
                await repository.UpdateManyAsync(config => names.Contains(config.Name), update);

                sentryClient.CaptureMessage($"Game {string.Join(", ", names)} enabled: {enabled}");

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateDisabledMessage(string name, Dictionary<string, string> messages, bool isApplyAll = false)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update.Set(field => field.DisabledMessage, messages[Language.DefaultLanguageValue]);
                var notificationUpdate = new UpdateRequest
                {
                    DisplayKind = NotificationDisplayKind.DisabledGame,
                    Messages = messages,
                    GameName = name
                };

                if (string.IsNullOrWhiteSpace(name))
                {
                    await repository.UpdateManyAsync(config => true, update);

                    var games = await Get();
                    foreach (var game in games)
                    {
                        notificationUpdate.GameName = game.Info.Name;
                        await notificationService.AddOrUpdateForGame(notificationUpdate);
                    }
                }
                else
                {
                    await repository.UpdateAsync(config => config.Name == name, update);
                    await notificationService.AddOrUpdateForGame(notificationUpdate);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateBetLimit(string name, BetLimit betLimit)
        {
            try
            {
                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetBetLimitChangedModel(gameEntity, betLimit);
                sentryClient.CaptureChangedSettings(name, "Bot Setting", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update
                    .Set(field => field.Minbet, betLimit.MinBet)
                    .Set(field => field.Maxbet, betLimit.MaxBet)
                    .Set(field => field.MaxBetChoices, betLimit.MaxBetChoices);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateOdds(string name, Dictionary<string, double> odds)
        {
            try
            {
                if (odds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetOddsChangedModel(gameEntity.Odds, odds);
                sentryClient.CaptureChangedSettings(name, "Odds", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update.Set(field => field.Odds, odds);
                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateBotRatios(string name, double[] ratios)
        {
            try
            {
                if (ratios.Length < HoursOfDay)
                {
                    throw new InvalidOperationException($"{nameof(ratios)} not fullfill {HoursOfDay} hours.");
                }

                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetBotRatiosChangedModel(gameEntity.HoursMaxBot.Any() ? gameEntity.HoursMaxBot : new double[24], ratios);
                sentryClient.CaptureChangedSettings(name, "Bot Ratios", " (GMT-4)", changedSettings);

                var update = Builders<GameConfigEntity>.Update.Set(field => field.HoursMaxBot, ratios);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, double>> iconOdds)
        {
            try
            {
                if (iconOdds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetIconsChangedModel(gameEntity.SlotsConfig.Icons, iconOdds);
                sentryClient.CaptureChangedSettings(name, "Odds", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update.Set(field => field.SlotsConfig.Icons, iconOdds);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateCascadingSlotOdds(string name, CascadingSlotsConfig cascadingSlotsConfig)
        {
            try
            {
                if (cascadingSlotsConfig.OddsByCounts?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetCascadingSlotOddsChangedModel(gameEntity.CascadingSlotsConfig, cascadingSlotsConfig);
                sentryClient.CaptureChangedSettings(name, "Cascading Slot Odds Config", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update
                    .Set(field => field.CascadingSlotsConfig.OddsByCounts, cascadingSlotsConfig.OddsByCounts)
                    .Set(field => field.CascadingSlotsConfig.FreeSpins, cascadingSlotsConfig.FreeSpins);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels)
        {
            try
            {
                if (await IsDifferentStakes(name, stakeLevels))
                {
                    throw new InvalidOperationException($"Stakes of game {name} are different.");
                }

                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetJackpotChangedModel(gameEntity, jackpot, stakeLevels);
                sentryClient.CaptureChangedSettings(name, "Jackpot", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update
                    .Set(field => field.SlotsConfig.StakeLevels, stakeLevels)
                    .Set(field => field.SlotsConfig.Jackpot.ExcludeTestCurrency, jackpot.ExcludeTestCurrency)
                    .Set(field => field.SlotsConfig.Jackpot.BotContributionRatio, jackpot.BotContributionRatio)
                    .Set(field => field.SlotsConfig.Jackpot.PlayerContributionRatio, jackpot.PlayerContributionRatio)
                    .Set(field => field.SlotsConfig.Jackpot.WinningRatio, jackpot.WinningRatio)
                    .Set(field => field.SlotsConfig.Jackpot.AllowedCurrency, jackpot.AllowedCurrency);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        private async Task<bool> IsDifferentStakes(string name, List<StakeLevel> stakeLevels)
        {
            var gameConfig = await GetByName(name);

            var updatedStakes = stakeLevels.OrderBy(x => x.Stake).Select(x => x.Stake).ToArray();
            var currentStakes = gameConfig.SlotsConfig.StakeLevels.OrderBy(x => x.Stake).Select(x => x.Stake).ToArray();
            var diffs = updatedStakes.Except(currentStakes);

            return diffs.Any();
        }

        public async Task<bool> UpdateSlotConfig(string name, bool botEnabled, SlotsConfig slotsConfig)
        {
            try
            {
                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetSlotConfigChangedModel(gameEntity, botEnabled, slotsConfig);
                sentryClient.CaptureChangedSettings(name, "Slot Config", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update
                            .Set(field => field.Botenabled, botEnabled)
                            .Set(field => field.SlotsConfig.Bonus, slotsConfig.Bonus)
                            .Set(field => field.SlotsConfig.BigWinRatio, slotsConfig.BigWinRatio)
                            .Set(field => field.SlotsConfig.Mode, slotsConfig.Mode);
                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateCascadingSlotsConfig(string name, bool botEnabled, CascadingSlotsConfig cascadingSlotsConfig)
        {
            try
            {
                var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);
                var changedSettings = SettingHelper.GetCascadingSlotConfigsChangeModel(gameEntity, botEnabled, cascadingSlotsConfig);
                sentryClient.CaptureChangedSettings(name, "Cascading Slot Config", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update
                       .Set(field => field.Botenabled, botEnabled)
                       .Set(field => field.CascadingSlotsConfig.MultiplierBonus, cascadingSlotsConfig.MultiplierBonus)
                       .Set(field => field.CascadingSlotsConfig.BigWinRatio, cascadingSlotsConfig.BigWinRatio)
                       .Set(field => field.CascadingSlotsConfig.Mode, cascadingSlotsConfig.Mode)
                       .Set(field => field.CascadingSlotsConfig.BetMultiplier, cascadingSlotsConfig.BetMultiplier)
                       .Set(field => field.CascadingSlotsConfig.BetLevels, cascadingSlotsConfig.BetLevels);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateGeneralConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting)
        {
            try
            {
                var gameEntity = await repository.FindOneAsync(entity => entity.Name == gameInfo.Name);
                var changedSettings = SettingHelper.GetGeneralConfigChangedModel(gameEntity, gameInfo, botSetting, enableSetting);
                sentryClient.CaptureChangedSettings(gameInfo.Name, "General Config", string.Empty, changedSettings);

                var update = Builders<GameConfigEntity>.Update
                    .Set(field => field.EnableFreeBet, gameInfo.EnableFreeBet)
                    .Set(field => field.DelayStartTime, enableSetting.DelayStartTime)
                    .Set(field => field.Botenabled, botSetting.BotEnabled)
                    .Set(field => field.BotMaxBet, botSetting.BotMaxBet)
                    .Set(field => field.SmartBotEnabled, botSetting.SmartBotEnabled)
                    .Set(field => field.SmartBotGroupRatio, botSetting.SmartBotGroupRatio)
                    .Set(field => field.RandomMode, gameInfo.RandomMode);

                await repository.UpdateAsync(config => config.Name == gameInfo.Name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<Dictionary<string, string>> GetDisabledMessage(string name)
        {
            var notification = await notificationService.GetByGameName(name, NotificationDisplayKind.DisabledGame);

            if (notification != null)
            {
                return notification.Messages;
            }

            var gameConfig = await GetByName(name);

            return new Dictionary<string, string> { { Language.DefaultLanguageValue, gameConfig.EnableSetting.DisabledMessage } };
        }

        public async Task<bool> UpdateSlotRNGConfig(GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting, SlotRNGConfig slotRNGConfig)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.Name == gameInfo.Name);
            var changedSettings = SettingHelper.GetSlotRNGConfigsChangeModel(gameEntity, gameInfo, botSetting, enableSetting, slotRNGConfig);
            sentryClient.CaptureChangedSettings(gameInfo.Name, "General Config", string.Empty, changedSettings);

            var update = Builders<GameConfigEntity>.Update
                .Set(field => field.EnableFreeBet, gameInfo.EnableFreeBet)
                .Set(field => field.DelayStartTime, enableSetting.DelayStartTime)
                .Set(field => field.SlotRNGConfig.Mode, slotRNGConfig.Mode)
                .Set(field => field.SlotRNGConfig.LuckySpinFrom, slotRNGConfig.LuckySpinFrom)
                .Set(field => field.SlotRNGConfig.LuckySpinTo, slotRNGConfig.LuckySpinTo)
                .Set(field => field.Botenabled, botSetting.BotEnabled)
                .Set(field => field.BotMaxBet, botSetting.BotMaxBet)
                .Set(field => field.SmartBotEnabled, botSetting.SmartBotEnabled)
                .Set(field => field.SmartBotGroupRatio, botSetting.SmartBotGroupRatio)
                .Set(field => field.RandomMode, gameInfo.RandomMode);

            await repository.UpdateAsync(config => config.Name == gameInfo.Name, update);

            return true;
        }
    }
}