using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISiteRepository : IGenericRepository<ClientSiteEntity>
    {
        Task UpdateGameClientUrlAsync(string clientId, string siteId, string gameClientUrl, string mobiClientUrl);
    }

    public class SiteRepository : GenericRepository<ClientSiteEntity>, ISiteRepository
    {
        public SiteRepository(IMongoClient client) : base(client.GetDatabase("member"))
        {
        }

        public async Task UpdateGameClientUrlAsync(string clientId, string siteId, string gameClientUrl, string mobiClientUrl)
        {
            var updates = Builders<ClientSiteEntity>.Update
                .Set(x => x.GameClientUrl, gameClientUrl)
                .Set(x => x.GameMobiClientUrl, mobiClientUrl);

            await UpdateAsync(x => x.ClientId == clientId && x.SiteId == siteId, updates);
        }
    }
}