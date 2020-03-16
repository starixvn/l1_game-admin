using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.Games.Entities;
using GamesAdmin.Core.Models;
using MongoDB.Driver;

namespace GamesAdmin.Api.Games
{
    public interface IGameService
    {
        IList<Game> Get();

        Game Get(string id);

        bool Create(Game game);

        void Update(string id, Game game);
    }

    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameEntity> games;

        public GameService(IDatabaseSettings settings, MongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            games = database.GetCollection<GameEntity>(settings.GameConfigsCollectionName);
        }

        public IList<Game> Get()
        {
            var gameEntities = games.Find(game => true).ToList();

            return gameEntities
                .Select(entity => new Game(
                    entity.Id,
                    entity.name,
                    entity.enabled))
                .ToList();
        }

        public Game Get(string id)
        {
            var gameEntity = games
            .Find(entity => entity.Id == id)
            .FirstOrDefault();

            return new Game(
                    gameEntity.Id,
                    gameEntity.name,
                    gameEntity.enabled);
        }
       

        public bool Create(Game game)
        {
            games.InsertOne(new GameEntity 
            { 
                Id = game.Id, 
                name = game.Name, 
                enabled = game.Enabled 
            });
            
            return true;
        }

        public void Update(string id, Game game) =>
            games
            .ReplaceOne(entity => entity.Id == id, 
                new GameEntity {Id = game.Id, name = game.Name, enabled = game.Enabled });
    }
}
