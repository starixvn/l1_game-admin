using System.Collections.Generic;

using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class GetMatchConfigRequest : IRequest<IEnumerable<MatchConfig>>
    {
        public GetMatchConfigRequest()
        {
        }
    }
}
