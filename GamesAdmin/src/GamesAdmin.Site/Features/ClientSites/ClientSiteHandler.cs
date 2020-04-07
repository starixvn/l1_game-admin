using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Site.Features.ClientSites.Requests;
using GamesAdmin.Site.Features.ClientSites.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites
{
    public class ClientSiteHandler
        : IRequestHandler<GetAllRequest, IEnumerable<ClientSiteViewModel>>
    {
        private readonly IClientSiteService clientSiteService;

        public ClientSiteHandler(IClientSiteService clientSiteService)
        {
            this.clientSiteService = clientSiteService;
        }

        public async Task<IEnumerable<ClientSiteViewModel>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            var result = await clientSiteService.GetAllAsync();

            return result.Select(x => new ClientSiteViewModel
            {
                ClientId = x.ClientId,
                SiteId = x.SiteId,
                GameClientUrl = x.GameClientUrl
            });
        }
    }
}