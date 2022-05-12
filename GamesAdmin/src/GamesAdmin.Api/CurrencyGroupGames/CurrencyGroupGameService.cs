using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Currencies;
using MongoDB.Bson;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.CurrencyGroupGames
{
    public interface ICurrencyGroupGameService
    {
        Task<IEnumerable<CurrencyGroupGame>> GetAll();

        Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId);

        Task<bool> Update(CurrencyGroupGame currencyGroupGame);

        Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId);
    }

    public class CurrencyGroupGameService : ICurrencyGroupGameService
    {
        private readonly IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository;
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;

        public CurrencyGroupGameService(
            IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository,
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository)
        {
            this.currencyGroupGameRepository = currencyGroupGameRepository;
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.currencyGroupRepository = currencyGroupRepository;
        }

        public async Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId)
        {
            try
            {
                var gameConfig = await currencyGroupGameRepository.FindOneAsync(entity => entity.GameId == gameId && entity.GroupId == ObjectId.Parse(currencyGroupId));

                return mapper.Map<CurrencyGroupGame>(gameConfig);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId)
        {
            try
            {
                var gameConfigs = await currencyGroupGameRepository.FilterByAsync(entity => entity.GroupId == ObjectId.Parse(currencyGroupId));

                return gameConfigs.Select(gameConfig => mapper.Map<CurrencyGroupGame>(gameConfig));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public Task<IEnumerable<CurrencyGroupGame>> GetAll()
        => Task.Run(() =>
        {
            try
            {
                var currencyGroupGames = currencyGroupGameRepository.AsQueryable().ToList();

                return currencyGroupGames.Select(entity => mapper.Map<CurrencyGroupGame>(entity));
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);

                return Enumerable.Empty<CurrencyGroupGame>();
            }
        });

        public async Task<bool> Update(CurrencyGroupGame currencyGroupGame)
        {
            try
            {
                var gameConfig = await currencyGroupGameRepository.FindOneAsync(entity => entity.GameId == currencyGroupGame.GameId && entity.GroupId == ObjectId.Parse(currencyGroupGame.GroupId));

                if (gameConfig == null)
                {
                    var entity = mapper.Map<CurrencyGroupGameEntity>(currencyGroupGame);
                    await currencyGroupGameRepository.InsertOneAsync(entity);
                }
                else
                {
                    var game = Game.FromGameId((GameId)currencyGroupGame.GameId);
                    var group = await currencyGroupRepository.FindByIdAsync(currencyGroupGame.GroupId);
                    var changedSettings = SettingHelper.GetCurrencyGroupGameChangedModel(gameConfig, currencyGroupGame);
                    sentryClient.CaptureChangedSettings(game.Value, "Partner's game setting", $" ({group.PartnerId} - {group.Name})", changedSettings);

                    var update = Builders<CurrencyGroupGameEntity>.Update
                        .Set(field => field.Enabled, currencyGroupGame.Enabled)
                        .Set(field => field.MinBet, currencyGroupGame.MinBet)
                        .Set(field => field.MaxBet, currencyGroupGame.MaxBet)
                        .Set(field => field.MaxBetPerChoices, currencyGroupGame.MaxBetPerChoices)
                        .Set(field => field.Chips, currencyGroupGame.Chips);

                    if (gameConfig.StakeLevels == null)
                    {
                        update.Set(field => field.StakeLevels, currencyGroupGame.StakeLevels);
                    }

                    await currencyGroupGameRepository.UpdateAsync(entity => entity.GroupId == ObjectId.Parse(currencyGroupGame.GroupId)
                        && entity.GameId == currencyGroupGame.GameId, update);
                }

                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }
    }
}