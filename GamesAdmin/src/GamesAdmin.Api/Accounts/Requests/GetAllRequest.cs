using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Accounts.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<AccountInfo>>
    {
    }
}
