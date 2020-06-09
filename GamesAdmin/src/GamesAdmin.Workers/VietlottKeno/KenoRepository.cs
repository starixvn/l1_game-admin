using System.Threading.Tasks;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.VietlottKeno
{
    public interface IKenoRepository : IGenericRepository<ProVietKenoRoundEntity>
    {
        Task UpdateAsync(ProVietKenoRoundEntity entity, UpdateOptions updateOptions = null);

        Task<ProVietKenoRoundEntity> GetLatestRoundAsync();
    }

    public class KenoRepository : GenericRepository<ProVietKenoRoundEntity>, IKenoRepository
    {
        private readonly IMongoCollection<ProVietKenoRoundEntity> collection;

        public KenoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            collection = mongoDatabase.GetCollection<ProVietKenoRoundEntity>(Document.GetCollectionName(typeof(ProVietKenoRoundEntity)));
        }

        public async Task UpdateAsync(ProVietKenoRoundEntity entity, UpdateOptions updateOptions = null)
        {
            var updateDefinition = Builders<ProVietKenoRoundEntity>.Update;

            await collection.UpdateOneAsync(x => x.Number == entity.Number,
                    updateDefinition.Combine(
                        updateDefinition.Set(x => x.Time, entity.Time),
                        updateDefinition.Set(x => x.Result, entity.Result),
                        updateDefinition.Set(x => x.SettlementResult, entity.SettlementResult)
                        ),
                    updateOptions
                );
        }

        public async Task<ProVietKenoRoundEntity> GetLatestRoundAsync()
        {
            return await collection.Find(Builders<ProVietKenoRoundEntity>.Filter.Empty)
                .SortByDescending(x => x.Number)
                .Limit(1).FirstOrDefaultAsync();
        }
    }
}