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

            games = database.GetCollection<GameEntity>(settings.GamesCollectionName);
        }

        public IList<Game> Get()
        {
            var gameEntities = games.Find(game => true).ToList();

            return gameEntities
                .Select(entity => new Game(
                    entity.ObjectId,
                    entity.Id,
                    entity.Name,
                    entity.Enabled))
                .ToList();
        }

        public Game Get(string id)
        {
            var gameEntity = games
            .Find(entity => entity.ObjectId == id)
            .FirstOrDefault();

            return new Game(
                    gameEntity.ObjectId,
                    gameEntity.Id,
                    gameEntity.Name,
                    gameEntity.Enabled);
        }
       

        public bool Create(Game game)
        {
            games.InsertOne(new GameEntity 
            { 
                Id = game.Id, 
                Name = game.Name, 
                Enabled = game.Enabled 
            });
            
            return true;
        }

        public void Update(string id, Game game) =>
            games
            .ReplaceOne(entity => entity.ObjectId == id, 
                new GameEntity { ObjectId = id, Id = game.Id, Name = game.Name, Enabled = game.Enabled });
    }
}
