using System.Threading.Tasks;
using GamesAdmin.Workers.Repository.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Workers.Repository
{
    public interface IKenoRepository
    {
        Task InsertAsync(KenoRoundEntity entity);
    }

    public class KenoRepository : IKenoRepository
    {
        private const string databaseName = "minigame";
        private const string collectionName = "kenovietlott_phiens";
        private readonly IMongoCollection<KenoRoundEntity> mongoCollection;

        public KenoRepository(IMongoClient mongoClient)
        {
            mongoCollection = mongoClient.GetDatabase(databaseName).GetCollection<KenoRoundEntity>(collectionName);
        }

        public async Task InsertAsync(KenoRoundEntity entity)
        {
            await mongoCollection.InsertOneAsync(entity);
        }
    }
}