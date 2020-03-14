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
    }

    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameEntity> games;

        public GameService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            games = database.GetCollection<GameEntity>(settings.GamesCollectionName);
        }

        public IList<Game> Get()
        {
            var gameEntities = games.Find(game => true).ToList();

            return gameEntities.Select(entity => new Game(entity.Id, entity.Name, entity.Enabled)).ToList();
        }
        
    }
}
