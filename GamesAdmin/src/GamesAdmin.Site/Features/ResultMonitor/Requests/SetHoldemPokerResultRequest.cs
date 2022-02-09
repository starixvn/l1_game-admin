using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models.RoundResults;
using MediatR;

namespace GamesAdmin.Site.Features.ResultMonitor.Requests
{
    public class SetHoldemPokerResultRequest : IRequest<HoldemPokerResultControl>
    {
        public string gameConfigName { get; set; }

        public string roundName { get; set; }

        public PokerCard[] player { get; set; }

        public PokerCard[] dealer { get; set; }

        public PokerCard[] communityCards { get; set; }
    }
}