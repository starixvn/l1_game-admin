using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Users.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<User>>
    {
    }
}
