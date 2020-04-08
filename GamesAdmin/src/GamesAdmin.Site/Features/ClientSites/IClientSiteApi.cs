using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Site.Features.ClientSites
{
    [Headers("Authorization: Bearer")]
    public interface IClientSiteApi
    {
        [Get("/ClientSites")]
        Task<IEnumerable<ClientSite>> GetAll();

        [Get("/ClientSites/{clientId}-{siteId}")]
        Task<ClientSite> Get(string clientId, string siteId);

        [Post("/ClientSites/update")]
        Task<bool> Update(ClientSite clientSite);
    }
}