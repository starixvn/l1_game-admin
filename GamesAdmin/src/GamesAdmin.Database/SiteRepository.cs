using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Database.Entities;
using MongoDB.Driver;

namespace GamesAdmin.Database
{
    public interface ISiteRepository : IGenericRepository<ClientSiteEntity>
    {
        Task UpdateGameClientUrlAsync(string clientId, string siteId, string gameClientUrl, string mobiClientUrl, IEnumerable<string> currencies);
    }

    public class SiteRepository : GenericRepository<ClientSiteEntity>, ISiteRepository
    {
        public SiteRepository(IMongoClient client) : base(client.GetDatabase("member"))
        {
        }

        public async Task UpdateGameClientUrlAsync(string clientId, string siteId, string gameClientUrl, string mobiClientUrl, IEnumerable<string> currencies)
        {
            var updates = Builders<ClientSiteEntity>.Update
                .Set(x => x.GameClientUrl, gameClientUrl)
                .Set(x => x.GameMobiClientUrl, mobiClientUrl)
                .Set(x => x.ValidCurrencies, currencies);

            await UpdateAsync(x => x.ClientId == clientId && x.SiteId == siteId, updates);
        }
    }
}