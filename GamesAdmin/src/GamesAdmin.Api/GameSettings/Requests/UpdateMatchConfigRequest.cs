using System.Collections.Generic;

using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateMatchConfigRequest : IRequest<bool>
    {
        public UpdateMatchConfigRequest(IEnumerable<MatchConfig> matchConfigs)
        {
            MatchConfigs = matchConfigs;
        }

        public IEnumerable<MatchConfig> MatchConfigs { get; set; }
    }
}
