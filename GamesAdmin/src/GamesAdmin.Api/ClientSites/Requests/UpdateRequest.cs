using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ClientSites.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(ClientSite clientSite)
        {
            ClientSite = clientSite;
        }

        public ClientSite ClientSite { get; }
    }
}