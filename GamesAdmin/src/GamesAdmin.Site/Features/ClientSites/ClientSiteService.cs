using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Refit;

namespace GamesAdmin.Site.Features.ClientSites
{
    public interface IClientSiteService
    {
        Task<IEnumerable<ClientSite>> GetAllAsync();
    }

    public class ClientSiteService: IClientSiteService
    {
        private readonly IClientSiteApi clientSiteApi;

        public ClientSiteService(HttpClient httpClient)
        {
            clientSiteApi = RestService.For<IClientSiteApi>(httpClient);
        }

        public Task<IEnumerable<ClientSite>> GetAllAsync()
        => clientSiteApi.GetAll();
    }
}