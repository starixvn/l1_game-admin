using System.Collections.Generic;
using GamesAdmin.Site.Features.ClientSites.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ClientSites.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<ClientSiteViewModel>>
    {
    }
}