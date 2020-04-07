using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ClientSites.Requests
{
    public class GetRequest : IRequest<ClientSite>
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