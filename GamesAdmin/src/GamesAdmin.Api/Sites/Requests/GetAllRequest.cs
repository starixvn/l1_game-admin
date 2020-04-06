using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Sites.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<Site>>
    {
    }
}