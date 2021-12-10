using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateBetLimitRequest : IRequest<bool>
    {
        public UpdateBetLimitRequest()
        {
        }

        public UpdateBetLimitRequest(
            string name,
            GameType type,
            BetLimit betLimit)
        {
            Name = name;
            Type = type;
            BetLimit = betLimit;
        }

        public string Name { get; set; }

        public GameType Type { get; set; }

        public BetLimit BetLimit { get; set; }
    }
}