using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class UpdateSiteExclusionRequest : IRequest<bool>
    {
        public UpdateSiteExclusionRequest(string partnerId, int gameId, string[] exclusionSites, IEnumerable<int> otherGameIdsInGroup)
        {
            PartnerId = partnerId;
            GameId = gameId;
            ExclusionSites = exclusionSites;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string PartnerId { get; set; }

        public int GameId { get; set; }

        public string[] ExclusionSites { get; set; }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}