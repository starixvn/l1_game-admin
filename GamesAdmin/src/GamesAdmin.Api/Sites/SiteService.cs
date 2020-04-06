using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Database;
using Sentry;

namespace GamesAdmin.Api.Sites
{
    public interface ISiteSerivce
    {
        Task<bool> UpdateAsync(Site site);

        Task<IEnumerable<Site>> GetAllAsync();
    }

    public class SiteSerivce : ISiteSerivce
    {
        private readonly ISiteRepository siteRepository;
        private readonly ISentryClient sentryClient;

        public SiteSerivce(
            ISentryClient sentryClient,
            ISiteRepository siteRepository)
        {
            this.sentryClient = sentryClient;
            this.siteRepository = siteRepository;
        }

        public async Task<IEnumerable<Site>> GetAllAsync()
        {
            var result = await siteRepository.FilterByAsync(x => true);
            return result.Select(x => new Site { ClientId = x.ClientId, SiteId = x.SiteId, GameClientUrl = x.GameClientUrl });
        }

        public async Task<bool> UpdateAsync(Site site)
        {
            await siteRepository.UpdateGameClientUrlAsync(site.ClientId, site.SiteId, site.GameClientUrl);

            return true;
        }
    }
}