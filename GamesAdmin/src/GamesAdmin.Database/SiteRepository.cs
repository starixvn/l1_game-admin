using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISiteRepository : IGenericRepository<SiteEntity>
    {
        Task UpdateGameClientUrlAsync(string clientId, string siteId, string gameClientUrl);
    }

    public class SiteRepository : GenericRepository<SiteEntity>, ISiteRepository
    {
        public SiteRepository(IMongoClient client) : base(client.GetDatabase("member"))
        {
        }

        public async Task UpdateGameClientUrlAsync(string clientId, string siteId, string gameClientUrl)
        {
            await UpdateAsync(x => x.ClientId == clientId && x.SiteId == siteId, Builders<SiteEntity>.Update.Set(x => x.GameClientUrl, gameClientUrl));
        }
    }
}