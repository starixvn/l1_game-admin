using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        Task<bool> Update(GameConfig game);

        Task<GameConfig> GetByName(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisbledMessage(string name, string disabledMessage, bool isApplyAll = false);

        Task<bool> UpdateBetLimitAndOdds(string name, BetLimit betLimit, Dictionary<string, double> odds);

        Task<bool> UpdateBotRatios(string name, double[] ratios);

        Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, float>> iconOdds);

        Task<bool> UpdateJackpot(string name, Jackpot jackpot, List<StakeLevel> stakeLevels);

        Task<bool> UpdateSlotConfig(string name, bool botEnabled, SlotsConfig slotsConfig);

        Task<bool> UpdateGeneralSetting(string name, GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting);
    }

    public class GameSettingsService : IGameSettingsService
    {
        private const byte HoursOfDay = 24;

        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<GameConfigEntity> repository;

        public GameSettingsService(
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<GameConfigEntity> repository)
        {
            this.repository = repository;
            this.sentryClient = sentryClient;
            this.mapper = mapper;
        }

        public Task<IEnumerable<GameConfig>> Get()
        => Task.Run(() =>
            {
                try
                {
                    var gameEntities = repository.AsQueryable().ToList();

                    return gameEntities
                        .Select(entity => mapper.Map<GameConfig>(entity));
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);

                    return Enumerable.Empty<GameConfig>();
                }
            });

        public async Task<bool> Create(GameConfig game)
        {
            try
            {
                var entity = new GameConfigEntity
                {
                    Name = game.Info.Name,
                    DisplayName = game.Info.DisplayName,
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
                    Time = DateTime.Now
                };

                await repository.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }

            return true;
        }

        public async Task<bool> Update(GameConfig game)
        {
            try
            {
                var existEntity = await GetByName(game.Info.Name);

                var entity = mapper.Map<GameConfigEntity>(game);
                entity.Time = DateTime.Now;

                if (entity.HoursMaxBot == null || entity.HoursMaxBot.Length < HoursOfDay)
                {
                    entity.HoursMaxBot = existEntity.BotSetting.HoursMaxBot;
                }

                await repository.ReplaceOneAsync(entity);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
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
                var update = Builders<GameConfigEntity>.Update.Set(nameof(GameConfigEntity.Enabled), enabled);
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

        public async Task<bool> UpdateDisbledMessage(string name, string disabledMessage, bool isApplyAll = false)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update.Set(nameof(GameConfigEntity.DisabledMessage), disabledMessage);

                if (isApplyAll)
                {
                    await repository.UpdateManyAsync(config => true, update);
                }
                else
                {
                    await repository.UpdateAsync(config => config.Name == name, update);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateBetLimitAndOdds(string name, BetLimit betLimit, Dictionary<string, double> odds)
        {
            try
            {
                if (odds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var update = Builders<GameConfigEntity>.Update
                    .Set(nameof(GameConfigEntity.Odds), odds)
                    .Set(nameof(GameConfigEntity.Minbet), betLimit.MinBet)
                    .Set(nameof(GameConfigEntity.Maxbet), betLimit.MaxBet)
                    .Set(nameof(GameConfigEntity.MaxBetChoices), betLimit.MaxBetChoices);

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

                var update = Builders<GameConfigEntity>.Update.Set(nameof(GameConfigEntity.HoursMaxBot), ratios);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateSlotIconOdds(string name, Dictionary<string, Dictionary<string, float>> iconOdds)
        {
            try
            {
                if (iconOdds?.Count == 0)
                {
                    throw new InvalidOperationException($"Odds of game {name} are empty.");
                }

                var update = Builders<GameConfigEntity>.Update.Set(nameof(GameConfigEntity.SlotsConfig.Icons), iconOdds);

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

                var update = Builders<GameConfigEntity>.Update
                    .Set(nameof(GameConfigEntity.SlotsConfig.Jackpot.BotContributionRatio), jackpot.BotContributionRatio)
                    .Set(nameof(GameConfigEntity.SlotsConfig.Jackpot.PlayerContributionRatio), jackpot.PlayerContributionRatio)
                    .Set(nameof(GameConfigEntity.SlotsConfig.Jackpot.WinningAmount), jackpot.WinningAmount);

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
            var diffs = updatedStakes.Intersect(currentStakes);

            return diffs.Any();
        }

        public async Task<bool> UpdateSlotConfig(string name, bool botEnabled, SlotsConfig slotsConfig)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update
                    .Set(nameof(GameConfigEntity.SlotsConfig.Bonus), slotsConfig.Bonus)
                    .Set(nameof(GameConfigEntity.SlotsConfig.BigWinRatio), slotsConfig.BigWinRatio)
                    .Set(nameof(GameConfigEntity.SlotsConfig.Mode), slotsConfig.Mode)
                    .Set(nameof(GameConfigEntity.Botenabled), botEnabled);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> UpdateGeneralSetting(string name, GameInfo gameInfo, BotSetting botSetting, EnableSetting enableSetting)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update
                    .Set(nameof(GameConfigEntity.DisplayName), gameInfo.DisplayName)
                    .Set(nameof(GameConfigEntity.EnableFreeBet), gameInfo.EnableFreeBet)
                    .Set(nameof(GameConfigEntity.DelayStartTime), enableSetting.DelayStartTime)
                    .Set(nameof(GameConfigEntity.Botenabled), botSetting.BotEnabled)
                    .Set(nameof(GameConfigEntity.BotMaxBet), botSetting.BotMaxBet);

                await repository.UpdateAsync(config => config.Name == name, update);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }
    }
}