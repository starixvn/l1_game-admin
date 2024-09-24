using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateBetLimitRequest : IRequest<bool>
    {
        public UpdateBetLimitRequest()
        {
        }

        public UpdateBetLimitRequest(
            int gameId,
            string name,
            GameType type,
            BetLimit betLimit,
            IEnumerable<int> otherGameIdsInGroup)
        {
            GameId = gameId;
            Name = name;
            Type = type;
            BetLimit = betLimit;
            OtherGameIdsInGroup = otherGameIdsInGroup ?? Enumerable.Empty<int>();
        }
        public int GameId { get; set; }
        public string Name { get; set; }

        public GameType Type { get; set; }

        public BetLimit BetLimit { get; set; }
        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}