using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Api.Games
{
    public interface IGameSettingsService
    {
        Task<IEnumerable<GameConfig>> Get();

        Task<bool> Create(GameConfig game);

        Task<bool> Update(GameConfig game);

        Task<GameConfig> GetByName(string name);

        Task<bool> Delete(string name);

        Task<bool> DisableAll(DateTimeOffset? disableAt);
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
                    name = game.Name,
                    enabled = game.Enabled,
                    maxbet = game.MaxBet,
                    minbet = game.MinBet,
                    botenabled = game.BotEnabled,
                    maxbot = game.MaxBot,
                    time = DateTime.Now
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
                var entity = mapper.Map<GameConfigEntity>(game);
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
            var gameEntity = await repository.FindOneAsync(entity => entity.name == name);

            return gameEntity != null ? mapper.Map<GameConfig>(gameEntity) : default;
        }

        public async Task<bool> Delete(string name)
        {
            try
            {
                await repository.DeleteOneAsync(entity => entity.name == name);

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return false;
            }
        }

        public async Task<bool> DisableAll(DateTimeOffset? disableAt)
        {
            try
            {
                var gameEntities = repository.AsQueryable().ToList();

                foreach (var game in gameEntities)
                {
                    game.enabled = false;

                    await repository.ReplaceOneAsync(game);
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
