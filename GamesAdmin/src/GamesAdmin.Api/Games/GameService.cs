using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api._Shared;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities.Game;
using MongoDB.Driver;

namespace GamesAdmin.Api.Games
{
    public interface IGameService
    {
        Task<IEnumerable<GameConfig>> Get();

        Task<GameConfig> Get(string id);

        Task<bool> Create(GameConfig game);

        Task<bool> Update(GameConfig game);

        Task<bool> Delete(string id);
    }

    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameConfigEntity> games;
        private readonly IMapper mapper;

        public GameService(
            IDatabaseSettings settings, 
            MongoClient mongoClient,
            IMapper mapper)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            games = database.GetCollection<GameConfigEntity>(settings.GameConfigsCollection);

            this.mapper = mapper;
        }

        public async Task<IEnumerable<GameConfig>> Get()
        {
            var gameEntities = await (games.AsQueryable()).ToListAsync();

            return gameEntities
                .Select(entity => mapper.Map<GameConfig>(entity));
        }

        public async Task<GameConfig> Get(string id)
        {
            var gameEntity = (await games
            .FindAsync(entity => entity.id == id))
            .FirstOrDefault();

            return mapper.Map<GameConfig>(gameEntity);
        }

        public async Task<bool> Create(GameConfig game)
        {
            await games.InsertOneAsync(mapper.Map<GameConfigEntity>(game));

            return true;
        }

        public async Task<bool> Update(GameConfig game)
        {
            try
            {
                await games.FindOneAndReplaceAsync(entity => entity.id == game.Id,
                    mapper.Map<GameConfigEntity>(game));

                return true;
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;

                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                await games.FindOneAndDeleteAsync(entity => entity.id == id);

                return true;
            }
            catch (Exception ex)
            {
                var errMsg = ex.Message;

                return false;
            }
        }
    }
}
