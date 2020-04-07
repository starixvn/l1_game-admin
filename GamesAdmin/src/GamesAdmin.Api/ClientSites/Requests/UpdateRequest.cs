using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ClientSites.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(ClientSite site)
        {
            Site = site;
        }

        public ClientSite Site { get; }
    }
}