using System.Collections.Generic;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateSlotIconOddsRequest : IRequest<bool>
    {
        public UpdateSlotIconOddsRequest(string name, Dictionary<string, Dictionary<string, float>> iconOdds)
        {
            Name = name;
            IconOdds = iconOdds;
        }

        public string Name { get; }

        public Dictionary<string, Dictionary<string, float>> IconOdds { get; }
    }
}