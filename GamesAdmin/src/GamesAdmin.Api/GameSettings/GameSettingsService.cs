using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
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

        Task<bool> Delete(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisbledMessage(string name, string disabledMessage, bool isApplyAll = false);
    }

    public class GameSettingsService : IGameSettingsService
    {
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

                if (entity.HoursMaxBot == null || entity.HoursMaxBot.Length < 24)
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

        public async Task<bool> Delete(string name)
        {
            try
            {
                await repository.DeleteOneAsync(entity => entity.Name == name);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
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
    }
}