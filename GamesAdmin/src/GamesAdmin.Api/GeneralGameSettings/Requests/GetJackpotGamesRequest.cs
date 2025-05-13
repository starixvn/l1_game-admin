
using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GeneralGameSettings.Requests
{
    public class GetJackpotGamesRequest : IRequest<IEnumerable<GeneralGameSetting>>
    {
    }
}
