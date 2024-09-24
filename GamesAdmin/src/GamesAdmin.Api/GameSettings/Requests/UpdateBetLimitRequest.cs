using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBetLimitRequest : IRequest<bool>
    {
        public UpdateBetLimitRequest(int gameId, string name, BetLimit betLimit, IEnumerable<int> otherGameIdsInGroup)
        {
            GameId = gameId;
            Name = name;
            BetLimit = betLimit;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public int GameId { get; }

        public string Name { get; }

        public BetLimit BetLimit { get; }
        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}