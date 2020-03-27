using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetAllRequest: IRequest<IEnumerable<GameConfig>>
    {
    }
}
