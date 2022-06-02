using System.Collections.Generic;
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
            CascadingSlotsConfig cascadingSlotsConfig)
        {
            Name = name;
            Type = type;
            Odds = odds;
            ClassicIcons = classicIcons;
            CascadingSlotsConfig = cascadingSlotsConfig;
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, Dictionary<string, double>> ClassicIcons { get; set; }

        public CascadingSlotsConfig CascadingSlotsConfig { get; set; }
    }
}