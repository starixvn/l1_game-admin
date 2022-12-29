using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GeneralGameSettings.Requests
{
    public class GetAllRequest : IRequest<IEnumerable<GeneralGameSetting>>
    {
    }
}
