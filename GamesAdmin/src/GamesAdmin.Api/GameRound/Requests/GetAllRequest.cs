using MediatR;
using System.Collections.Generic;
using GamesAdmin.Core.Models;

namespace GamesAdmin.Api.GameRound.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<Round>>
    {
    }
}
