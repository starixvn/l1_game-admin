using GamesAdmin.Database.Entities.DailySummary;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IDailySummaryRepository : IGenericRepository<DailySummaryEntity>
    {
    }

    public class DailySummaryRepository : GenericRepository<DailySummaryEntity>, IDailySummaryRepository
    {
        public DailySummaryRepository(IMongoClient client)
            : base(client.GetDatabase("summary"))
        {
        }
    }
}