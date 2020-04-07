using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ClientSites.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<ClientSite>>
    {
    }
}