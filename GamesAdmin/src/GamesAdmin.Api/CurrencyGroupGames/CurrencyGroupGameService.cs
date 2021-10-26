using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
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

        public CurrencyGroupGameService(IGenericRepository<CurrencyGroupGameEntity> currencyGroupGameRepository, IMapper mapper, ISentryClient sentryClient)
        {
            this.currencyGroupGameRepository = currencyGroupGameRepository;
            this.mapper = mapper;
            this.sentryClient = sentryClient;
        }

        public async Task<CurrencyGroupGame> Get(int gameId, string currencyGroupId)
        {
            try
            {
                var gameConfig = await currencyGroupGameRepository.FindOneAsync(entity => entity.GameId == gameId && entity.CurrencyGroupId == ObjectId.Parse(currencyGroupId));

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
                var gameConfigs = await currencyGroupGameRepository.FilterByAsync(entity => entity.CurrencyGroupId == ObjectId.Parse(currencyGroupId));

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
                var gameConfig = await currencyGroupGameRepository.FindOneAsync(entity => entity.GameId == currencyGroupGame.GameId && entity.CurrencyGroupId == ObjectId.Parse(currencyGroupGame.CurrencyGroupId));

                if (gameConfig != null)
                {
                    var update = Builders<CurrencyGroupGameEntity>.Update.Set(field => field.Enabled, currencyGroupGame.Enabled)
                        .Set(field => field.MinBet, currencyGroupGame.MinBet)
                        .Set(field => field.MaxBet, currencyGroupGame.MaxBet)
                        .Set(field => field.MaxBetPerChoices, currencyGroupGame.MaxBetPerChoices)
                        .Set(field => field.Chips, currencyGroupGame.Chips);

                    await currencyGroupGameRepository.UpdateAsync(entity => entity.CurrencyGroupId == ObjectId.Parse(currencyGroupGame.CurrencyGroupId)
                        && entity.GameId == currencyGroupGame.GameId, update);
                }
                else
                {
                    var entity = mapper.Map<CurrencyGroupGameEntity>(currencyGroupGame);

                    await currencyGroupGameRepository.InsertOneAsync(entity);
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