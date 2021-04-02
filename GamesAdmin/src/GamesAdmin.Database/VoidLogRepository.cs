using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface IVoidLogRepository : IGenericRepository<VoidLogEntity>
    {
    }

    public class VoidLogRepository : GenericRepository<VoidLogEntity>, IVoidLogRepository
    {
        public VoidLogRepository(IMongoClient client) : base(client.GetDatabase("log"))
        {
        }
    }
}