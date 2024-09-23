using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateOddsRequest : IRequest<bool>
    {
        public UpdateOddsRequest(string name, Dictionary<string, double> odds, IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            Odds = odds;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public Dictionary<string, double> Odds { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}