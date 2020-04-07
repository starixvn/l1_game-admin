using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites.Requests
{
    public class UpdateRequest : IRequest<ResultBase>
    {
        public UpdateRequest(ClientSite clientSite)
        {
            ClientSite = clientSite;
        }

        public ClientSite ClientSite { get; }
    }
}