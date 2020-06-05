using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.VietlottKeno
{
    public interface IKenoRepository : IGenericRepository<VietlottKenoRoundEntity>
    {
        Task UpdateAsync(VietlottKenoRoundEntity entity, UpdateOptions updateOptions = null);

        Task<VietlottKenoRoundEntity> GetLatestRoundAsync();
    }

    public class KenoRepository : GenericRepository<VietlottKenoRoundEntity>, IKenoRepository
    {
        private readonly IMongoCollection<VietlottKenoRoundEntity> collection;

        public KenoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            collection = mongoDatabase.GetCollection<VietlottKenoRoundEntity>(Document.GetCollectionName(typeof(VietlottKenoRoundEntity)));
        }

        public async Task UpdateAsync(VietlottKenoRoundEntity entity, UpdateOptions updateOptions = null)
        {
            var updateDefinition = Builders<VietlottKenoRoundEntity>.Update;

            await collection.UpdateOneAsync(x => x.Number == entity.Number,
                    updateDefinition.Combine(
                        updateDefinition.Set(x => x.Time, entity.Time),
                        updateDefinition.Set(x => x.Result, entity.Result),
                        updateDefinition.Set(x => x.SettlementResult, entity.SettlementResult)
                        ),
                    updateOptions
                );
        }

        public async Task<VietlottKenoRoundEntity> GetLatestRoundAsync()
        {
            return await collection.Find(Builders<VietlottKenoRoundEntity>.Filter.Empty)
                .SortByDescending(x => x.Number)
                .Limit(1).FirstOrDefaultAsync();
        }
    }
}