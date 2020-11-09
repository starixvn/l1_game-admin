using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Games
{
    public interface IGameSettingsService
    {
        Task<IEnumerable<AutoRNGGameConfig>> Get();

        Task<bool> Create(AutoRNGGameConfig game);

        Task<bool> Update(AutoRNGGameConfig game);

        Task<AutoRNGGameConfig> GetByName(string name);

        Task<bool> Delete(string name);

        Task<bool> UpdateStatus(string[] names, bool enabled);

        Task<bool> UpdateDisbledMessage(string name, string disabledMessage);
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

        public Task<IEnumerable<AutoRNGGameConfig>> Get()
        => Task.Run(() =>
            {
                try
                {
                    var gameEntities = repository.AsQueryable().ToList();

                    return gameEntities
                        .Select(entity => mapper.Map<AutoRNGGameConfig>(entity));
                }
                catch (Exception ex)
                {
                    sentryClient.CaptureException(ex);

                    return Enumerable.Empty<AutoRNGGameConfig>();
                }
            });

        public async Task<bool> Create(AutoRNGGameConfig game)
        {
            try
            {
                var entity = new GameConfigEntity
                {
                    Name = game.Name,
                    Enabled = game.Enabled,
                    Maxbet = game.MaxBet,
                    Minbet = game.MinBet,
                    Botenabled = game.BotEnabled,
                    DisabledMessage = game.DisabledMessage,
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

        public async Task<bool> Update(AutoRNGGameConfig game)
        {
            try
            {
                var existEntity = await GetByName(game.Name);

                var entity = mapper.Map<GameConfigEntity>(game);
                entity.Time = DateTime.Now;

                if (entity.HoursMaxBot == null || entity.HoursMaxBot.Length < 24)
                {
                    entity.HoursMaxBot = existEntity.HoursMaxBot;
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

        public async Task<AutoRNGGameConfig> GetByName(string name)
        {
            var gameEntity = await repository.FindOneAsync(entity => entity.Name == name);

            return gameEntity != null ? mapper.Map<AutoRNGGameConfig>(gameEntity) : default;
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

        public async Task<bool> UpdateDisbledMessage(string name, string disabledMessage)
        {
            try
            {
                var update = Builders<GameConfigEntity>.Update.Set(nameof(GameConfigEntity.DisabledMessage), disabledMessage);
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
