using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Refit;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IClientSiteApi: IBaseAuthorizationApi
    {
        [Get("/ClientSites")]
        Task<IEnumerable<ClientSite>> GetAll();

        [Get("/ClientSites/{clientId}-{siteId}")]
        Task<ClientSite> Get(string clientId, string siteId);

        [Post("/ClientSites/update")]
        Task<bool> Update(ClientSite clientSite);
    }
}