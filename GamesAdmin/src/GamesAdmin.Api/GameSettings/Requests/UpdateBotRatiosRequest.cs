using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBotRatiosRequest : IRequest<bool>
    {
        public UpdateBotRatiosRequest(string name, double[] botRatios, IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            BotRatios = botRatios;
            OtherGameIdsInGroup = otherGameIdsInGroup;
        }

        public string Name { get; }

        public double[] BotRatios { get; }

        public IEnumerable<int> OtherGameIdsInGroup { get; }
    }
}