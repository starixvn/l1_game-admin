using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class GetAllRequest: IRequest<IEnumerable<GameConfig>>
    {
    }
}
