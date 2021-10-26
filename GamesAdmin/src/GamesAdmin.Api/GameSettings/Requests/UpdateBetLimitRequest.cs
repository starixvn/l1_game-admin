using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class UpdateBetLimitRequest : IRequest<bool>
    {
        public UpdateBetLimitRequest(string name, BetLimit betLimit)
        {
            Name = name;
            BetLimit = betLimit;
        }

        public string Name { get; }

        public BetLimit BetLimit { get; }
    }
}