using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared.Helpers;
using GamesAdmin.Api.SentryClient;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using GamesAdmin.Database.Entities.Currencies;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Sentry;

namespace GamesAdmin.Api.CurrencyGroupGames
{
    public interface ICurrencyGroupGameService
    {
        Task<IEnumerable<CurrencyGroupGame>> GetAll();

        Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId);

        Task<bool> Update(CurrencyGroupGame currencyGroupGame);

        Task<IEnumerable<CurrencyGroupGame>> GetByGroup(string currencyGroupId);

        Task<IEnumerable<CurrencyGroupGame>> GetByGameId(int gameId);

        Task<CurrencyGroupGameConfig> GetConfig(string groupId, int gameId, string type);

        Task<bool> UpdateConfig(string groupId, int gameId, object configValue, string type);
    }

    public class CurrencyGroupGameService : ICurrencyGroupGameService
    {
        private readonly IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository;
        private readonly IMapper mapper;
        private readonly ISentryClient sentryClient;
        private readonly IGenericRepository<CurrencyGroupEntity> currencyGroupRepository;
        private readonly IGenericRepository<GameConfigEntity> gameConfigRepository;
        private readonly IGenericRepository<CurrencyGroupGameConfigEntity> currencyGroupGameConfigRepository;

        public CurrencyGroupGameService(
            IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository,
            IMapper mapper,
            ISentryClient sentryClient,
            IGenericRepository<CurrencyGroupEntity> currencyGroupRepository,
            IGenericRepository<GameConfigEntity> gameConfigRepository,
            IGenericRepository<CurrencyGroupGameConfigEntity> currencyGroupGameConfigRepository)
        {
            this.currencyGroupGameRepository = currencyGroupGameRepository;
            this.mapper = mapper;
            this.sentryClient = sentryClient;
            this.currencyGroupRepository = currencyGroupRepository;
            this.gameConfigRepository = gameConfigRepository;
            this.currencyGroupGameConfigRepository = currencyGroupGameConfigRepository;
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

        public async Task<IEnumerable<CurrencyGroupGame>> GetByGameId(int gameId)
        {
            try
            {
                var gameConfigs = await currencyGroupGameRepository.FilterByAsync(entity => entity.GameId == gameId);

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
                var gameIds = new List<int>(currencyGroupGame.OtherGameIdsInGroup) { currencyGroupGame.GameId };
                var currencyGroupGames = (await currencyGroupGameRepository.FilterByAsync(
                    entity => gameIds.Contains(entity.GameId) && entity.GroupId == ObjectId.Parse(currencyGroupGame.GroupId))).ToList();
                var gameConfigs = (await gameConfigRepository.FilterByAsync(x => gameIds.Contains(x.GameId))).ToList();

                var writes = new List<WriteModel<CurrencyGroupGameEntity>>();

                foreach (var gameId in gameIds)
                {
                    var groupGameConfig = currencyGroupGames.FirstOrDefault(x => x.GameId == gameId);

                    if (groupGameConfig != null)
                    {
                        var group = await currencyGroupRepository.FindByIdAsync(groupGameConfig.GroupId.ToString());
                        var changedSettings = SettingHelper.GetCurrencyGroupGameChangedModel(groupGameConfig, currencyGroupGame);
                        sentryClient.CaptureChangedSettings(groupGameConfig.GameName, "Partner's game setting", $" ({group.PartnerId} - {group.Name})", changedSettings);
                    }

                    var gameConfig = gameConfigs.FirstOrDefault(x => x.GameId == gameId);
                    var sortedCoinValues = currencyGroupGame.CoinValues.OrderBy(x => x).ToArray();
                    var sortedBetLevels = currencyGroupGame.BetLevels.OrderBy(x => x).ToArray();

                    var update = Builders<CurrencyGroupGameEntity>.Update
                        .Set(field => field.Enabled, currencyGroupGame.Enabled)
                        .Set(field => field.MinBet, currencyGroupGame.MinBet)
                        .Set(field => field.MaxBet, currencyGroupGame.MaxBet)
                        .Set(field => field.MaxBetPerChoices, currencyGroupGame.MaxBetPerChoices)
                        .Set(field => field.Chips, currencyGroupGame.Chips)
                        .Set(field => field.CoinValues, sortedCoinValues)
                        .Set(field => field.DefaultCoinValue, currencyGroupGame.DefaultCoinValue ?? sortedCoinValues.FirstOrDefault())
                        .Set(field => field.DefaultBetLevel, currencyGroupGame.DefaultBetLevel ?? sortedBetLevels.FirstOrDefault())
                        .Set(field => field.BetLevels, currencyGroupGame.BetLevels.OrderBy(x => x).ToArray() ?? new double[10])
                        .Set(field => field.MaxWinPerRound, currencyGroupGame.MaxWinPerRound)
                        .Set(field => field.GameName, gameConfig?.Name ?? currencyGroupGame.GameName);

                    var filterBuilder = Builders<CurrencyGroupGameEntity>.Filter;
                    var filterDefinition = filterBuilder.And(
                        filterBuilder.Eq(x => x.GroupId, ObjectId.Parse(currencyGroupGame.GroupId)),
                        filterBuilder.Eq(x => x.GameId, gameId));

                    writes.Add(new UpdateOneModel<CurrencyGroupGameEntity>(filterDefinition, update) { IsUpsert = true });
                }

                await currencyGroupGameRepository.Collection().BulkWriteAsync(writes, new BulkWriteOptions { IsOrdered = false });
                return true;
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
            }

            return false;
        }

        public async Task<CurrencyGroupGameConfig> GetConfig(string groupId, int gameId, string type)
        {
            try
            {
                var config = await currencyGroupGameConfigRepository
                    .FindOneAsync(entity => entity.GameId == gameId && entity.GroupId == groupId && entity.Type == type);
                return mapper.Map<CurrencyGroupGameConfig>(config);
            }
            catch (Exception ex)
            {
                sentryClient.CaptureException(ex);
                return null;
            }
        }

        public async Task<bool> UpdateConfig(string groupId, int gameId, object configValue, string type)
        {
            try
            {
                var filterBuilder = Builders<CurrencyGroupGameConfigEntity>.Filter;
                var filterDefinition = filterBuilder.And(
                    filterBuilder.Eq(x => x.GameId, gameId),
                    filterBuilder.Eq(x => x.GroupId, groupId),
                    filterBuilder.Eq(x => x.Type, type));
                var update = Builders<CurrencyGroupGameConfigEntity>.Update
                    .Set(field => field.Value, JsonConvert.DeserializeObject<ExpandoObject>(configValue.ToString()));

                var result = await currencyGroupGameConfigRepository.Collection().UpdateOneAsync(filterDefinition, update);

                if (result.MatchedCount == 0)
                {
                    var newConfig = new CurrencyGroupGameConfigEntity
                    {
                        GameId = gameId,
                        GroupId = groupId,
                        Type = type,
                        Value = JsonConvert.DeserializeObject<ExpandoObject>(configValue.ToString())
                    };
                    await currencyGroupGameConfigRepository.InsertOneAsync(newConfig);
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