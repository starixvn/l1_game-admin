using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateBetLimitAndOddsRequest : IRequest<bool>
    {
        public UpdateBetLimitAndOddsRequest()
        {
        }

        public UpdateBetLimitAndOddsRequest(
            string name,
            GameType type,
            BetLimit betLimit,
            Dictionary<string, double> odds,
            Dictionary<string, Dictionary<string, double>> icons)
        {
            Name = name;
            Type = type;
            BetLimit = betLimit;
            Odds = odds;
            Icons = icons;
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public BetLimit BetLimit { get; set; }

        public Dictionary<string, double> Odds { get; set; }

        public Dictionary<string, Dictionary<string, double>> Icons { get; set; }
    }
}