using GamesAdmin.Core.Models.GameConfigs;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetMatchConfigRequest : IRequest<IEnumerable<MatchConfig>>
    {
    }
}