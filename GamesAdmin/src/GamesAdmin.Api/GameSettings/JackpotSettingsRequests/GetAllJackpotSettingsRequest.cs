
using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.JackpotSettingsRequests
{
    public class GetAllJackpotSettingsRequest : IRequest<IEnumerable<JackpotSettings>>
    {
    }
}
