using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Sites.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(Site site)
        {
            Site = site;
        }

        public Site Site { get; }
    }
}