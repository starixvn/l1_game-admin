using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCasinoHoldemPokerBetHistoryRequest : IRequest<CasinoHoldemPokerBetHistory>
    {
        public GetCasinoHoldemPokerBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
