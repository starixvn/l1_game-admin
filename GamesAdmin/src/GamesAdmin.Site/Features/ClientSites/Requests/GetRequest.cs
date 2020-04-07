using GamesAdmin.Site.Features.ClientSites.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites.Requests
{
    public class GetRequest : IRequest<ClientSiteViewModel>
    {
        public GetRequest(string clientId, string siteId)
        {
            ClientId = clientId;
            SiteId = siteId;
        }

        public string ClientId { get; }

        public string SiteId { get; }
    }
}