using System.Collections.Generic;
using System.Linq;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateOddsRequest : IRequest<string>
    {
        public UpdateOddsRequest() { }

        public UpdateOddsRequest(
            string name,
            GameType type,
            Dictionary<string, double> odds,
            Dictionary<string, Dictionary<string, double>> classicIcons,
            CascadingSlotsConfig cascadingSlotsConfig,
            IEnumerable<int> otherGameIdsInGroup)
        {
            Name = name;
            Type = type;
            Odds = odds;
            ClassicIcons = classicIcons;
            CascadingSlotsConfig = cascadingSlotsConfig;
            OtherGameIdsInGroup = otherGameIdsInGroup ?? Enumerable.Empty<int>();
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, Dictionary<string, double>> ClassicIcons { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set; }

        public IEnumerable<int> OtherGameIdsInGroup { get; set; }
    }
}