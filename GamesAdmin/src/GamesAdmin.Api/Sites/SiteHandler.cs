using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.Sites.Requests;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Sites
{
    public class SiteHandler
        : IRequestHandler<UpdateRequest, bool>,
        IRequestHandler<GetAllRequest, IEnumerable<Site>>
    {
        private readonly ISiteSerivce siteSerivce;

        public SiteHandler(ISiteSerivce siteSerivce)
        {
            this.siteSerivce = siteSerivce;
        }

        public async Task<bool> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            return await siteSerivce.UpdateAsync(request.Site);
        }

        public async Task<IEnumerable<Site>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            return await siteSerivce.GetAllAsync();
        }
    }
}