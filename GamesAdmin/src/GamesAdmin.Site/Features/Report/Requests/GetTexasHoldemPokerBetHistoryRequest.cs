using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetTexasHoldemPokerBetHistoryRequest : IRequest<TexasHoldemPokerBetHistory>
    {
        public GetTexasHoldemPokerBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
