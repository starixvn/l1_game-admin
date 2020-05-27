using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IClientSiteApi : IBaseAuthorizationApi
    {
        [Get("/client_sites")]
        Task<IEnumerable<ClientSite>> GetAll();

        [Get("/client_sites/{clientId}/{siteId}")]
        Task<ClientSite> Get(string clientId, string siteId);

        [Post("/client_sites/update")]
        Task<bool> Update(ClientSite clientSite);
    }
}