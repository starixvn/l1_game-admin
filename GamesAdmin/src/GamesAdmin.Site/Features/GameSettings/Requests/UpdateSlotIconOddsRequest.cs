using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Site.Features.GameSettings.Requests
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
