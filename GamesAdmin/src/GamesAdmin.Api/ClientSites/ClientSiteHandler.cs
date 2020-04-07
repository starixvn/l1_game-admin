using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.ClientSites;
using GamesAdmin.Api.ClientSites.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Sites
{
    public class ClientSiteHandler
        : IRequestHandler<UpdateRequest, bool>,
        IRequestHandler<GetAllRequest, IEnumerable<ClientSite>>
    {
        private readonly IClientSiteSiteSerivce siteSerivce;

        public ClientSiteHandler(IClientSiteSiteSerivce siteSerivce)
        {
            this.siteSerivce = siteSerivce;
        }

        public async Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            return await siteSerivce.UpdateAsync(request.Site);
        }

        public async Task<IEnumerable<ClientSite>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            return await siteSerivce.GetAllAsync();
        }
    }
}