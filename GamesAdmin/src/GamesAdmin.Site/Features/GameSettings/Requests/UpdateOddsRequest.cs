using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateOddsRequest : IRequest<bool>
    {
        public UpdateOddsRequest()
        {
        }

        public UpdateOddsRequest(
            string name,
            GameType type,
            Dictionary<string, double> odds,
            Dictionary<string, Dictionary<string, double>> icons,
            Dictionary<string, List<OddsByCount>> oddsByCounts)
        {
            Name = name;
            Type = type;
            Odds = odds;
            Icons = icons;
            OddsByCounts = oddsByCounts;
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, Dictionary<string, double>> Icons { get; set; }

        public Dictionary<string, List<OddsByCount>> OddsByCounts { get; set; }
    }
}