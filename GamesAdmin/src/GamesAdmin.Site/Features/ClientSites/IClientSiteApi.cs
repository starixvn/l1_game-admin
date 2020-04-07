using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IClientSiteApi
    {
        [Get("/ClientSites")]
        Task<IEnumerable<ClientSite>> GetAll();
    }
}