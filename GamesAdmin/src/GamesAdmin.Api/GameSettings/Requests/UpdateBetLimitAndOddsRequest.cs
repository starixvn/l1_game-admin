using System.Collections.Generic;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBetLimitAndOddsRequest : IRequest<bool>
    {
        public UpdateBetLimitAndOddsRequest(string name, BetLimit betLimit, Dictionary<string, double> odds)
        {
            Name = name;
            BetLimit = betLimit;
            Odds = odds;
        }

        public string Name { get; }

        public BetLimit BetLimit { get; }

        public Dictionary<string, double> Odds { get; }
    }
}