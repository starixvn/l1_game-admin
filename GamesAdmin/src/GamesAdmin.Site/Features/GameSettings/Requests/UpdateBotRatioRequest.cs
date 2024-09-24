using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateBotRatioRequest : IRequest<bool>
    {
        public UpdateBotRatioRequest(string name, double[] botRatios, IEnumerable<int> otherGameIdsInGroup) 
        {
            Name = name;
            BotRatios = botRatios;
            OtherGameIdsInGroup = otherGameIdsInGroup ?? Enumerable.Empty<int>();
        }

        public string Name { get; }

        public double[] BotRatios { get; }
        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}
