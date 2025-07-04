using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSlotIconOddsRequest : IRequest<bool>
    {
        public UpdateSlotIconOddsRequest(string name, Dictionary<string, Dictionary<string, double>> iconOdds, IEnumerable<int> otherGameIdsInGroup = null)
        {
            Name = name;
            IconOdds = iconOdds;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public Dictionary<string, Dictionary<string, double>> IconOdds { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}