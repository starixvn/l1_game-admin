using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IClientSiteService
    {
        Task<IEnumerable<ClientSite>> GetAllAsync();

        Task<ClientSite> GetAsync(string clientId, string siteId);

        Task<bool> Update(ClientSite clientSite);
    }

    public class ClientSiteService : IClientSiteService
    {
        private readonly IClientSiteApi clientSiteApi;

        public ClientSiteService(IClientSiteApi clientSiteApi)
        {
            this.clientSiteApi = clientSiteApi;
        }

        public async Task<IEnumerable<ClientSite>> GetAllAsync()
        => await clientSiteApi.GetAll();

        public async Task<ClientSite> GetAsync(string clientId, string siteId)
        => await clientSiteApi.Get(clientId, siteId);

        public async Task<bool> Update(ClientSite clientSite)
         => await clientSiteApi.Update(clientSite);
    }
}