using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Site.Features.ClientSites
{
    public class ClientSiteService
    {
        private readonly IClientSiteApi clientSiteApi;

        public ClientSiteService(IClientSiteApi clientSiteApi)
        {
            this.clientSiteApi = clientSiteApi;
        }

        public Task<IEnumerable<ClientSite>> GetAll()
        => clientSiteApi.GetAll();
    }
}