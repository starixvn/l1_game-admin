using System;
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

        Task<ClientSite> Get(string clientId, string siteId);
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
            var sites = result.Select(x => new ClientSite
            {
                ClientId = x.ClientId,
                SiteId = x.SiteId,
                GameClientUrl = x.GameClientUrl,
                GameMobiClientUrl = x.GameMobiClientUrl
            }).ToList();

            return sites;
        }

        public async Task<ClientSite> Get(string clientId, string siteId)
        {
            var clientSiteEntity = await siteRepository.FindOneAsync(x => x.ClientId == clientId && x.SiteId == siteId);

            return new ClientSite { 
                ClientId = clientSiteEntity.ClientId, 
                SiteId = clientSiteEntity.SiteId, 
                GameClientUrl = clientSiteEntity.GameClientUrl,
                GameMobiClientUrl = clientSiteEntity.GameMobiClientUrl
            };
        }

        public async Task<bool> UpdateAsync(ClientSite site)
        {
            try
            {
                await siteRepository.UpdateGameClientUrlAsync(site.ClientId, site.SiteId, site.GameClientUrl, site.GameMobiClientUrl);

                return true;
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
                return false;
            }
        }
    }
}