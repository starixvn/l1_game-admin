using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.GameUser.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<AccountInfo>>
    {
    }
}
