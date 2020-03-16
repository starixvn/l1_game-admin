using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities.Game;
using MongoDB.Driver;

namespace GamesAdmin.Api.Games
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> Get();

        Task<Game> Get(string id);

        Task<bool> Create(Game game);

        Task<bool> Update(Game game);

        Task<bool> Delete(string id);
    }

    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameConfig> games;

        public GameService(IDatabaseSettings settings, MongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            games = database.GetCollection<GameConfig>(settings.GameConfigsCollection);
        }

        public async Task<IEnumerable<Game>> Get()
        {
            var gameEntities = await (games.AsQueryable()).ToListAsync();

            return gameEntities
                .Select(entity => new Game(
                    entity.id,
                    entity.name,
                    entity.minbet,
                    entity.maxbet,
                    entity.enabled));
        }

        public async Task<Game> Get(string id)
        {
            var gameEntity = (await games
            .FindAsync(entity => entity.id == id))
            .FirstOrDefault();

            return new Game(
                    gameEntity.id,
                    gameEntity.name,
                    gameEntity.minbet,
                    gameEntity.maxbet,
                    gameEntity.enabled);
        }

        public async Task<bool> Create(Game game)
        {
            await games.InsertOneAsync(new GameConfig
            {
                name = game.Name,
                minbet = game.MinBet,
                maxbet = game.MaxBet,
                enabled = game.Enabled
            });

            return true;
        }

        public async Task<bool> Update(Game game)
        {
            try
            {
                await games.FindOneAndReplaceAsync(entity => entity.id == game.Id,
                            new GameConfig
                            {
                                id = game.Id,
                                name = game.Name,
                                minbet = game.MinBet,
                                maxbet = game.MaxBet,
                                enabled = game.Enabled
                            });

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
