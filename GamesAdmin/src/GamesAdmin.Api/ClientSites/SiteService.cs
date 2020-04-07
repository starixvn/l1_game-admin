using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using Sentry;

namespace GamesAdmin.Api.ClientSites
{
    public interface IClientSiteSiteSerivce
    {
        Task<bool> UpdateAsync(ClientSite site);

        Task<IEnumerable<ClientSite>> GetAllAsync();
    }

    public class ClientSiteSerivce : IClientSiteSiteSerivce
    {
        private readonly ISiteRepository siteRepository;
        private readonly ISentryClient sentryClient;

        public ClientSiteSerivce(
            ISentryClient sentryClient,
            ISiteRepository siteRepository)
        {
            this.sentryClient = sentryClient;
            this.siteRepository = siteRepository;
        }

        public async Task<IEnumerable<ClientSite>> GetAllAsync()
        {
            var result = await siteRepository.FilterByAsync(x => true);
            return result.Select(x => new ClientSite { ClientId = x.ClientId, SiteId = x.SiteId, GameClientUrl = x.GameClientUrl });
        }

        public async Task<bool> UpdateAsync(ClientSite site)
        {
            await siteRepository.UpdateGameClientUrlAsync(site.ClientId, site.SiteId, site.GameClientUrl);

            return true;
        }
    }
}