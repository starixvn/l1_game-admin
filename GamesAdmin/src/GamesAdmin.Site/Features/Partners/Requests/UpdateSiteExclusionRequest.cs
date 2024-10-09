using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class UpdateSiteExclusionRequest : IRequest<bool>
    {
        public UpdateSiteExclusionRequest(string partnerId, int gameId, string[] exclusionSites, IEnumerable<int> otherGameIdsInGroup)
        {
            PartnerId = partnerId;
            GameId = gameId;
            ExclusionSites = exclusionSites;
            OtherGameIdsInGroup = otherGameIdsInGroup ?? Enumerable.Empty<int>();
        }

        public string PartnerId { get; set; }

        public int GameId { get; set; }

        public string[] ExclusionSites { get; set; }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}