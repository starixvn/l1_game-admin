using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CasinoHoldemPokerBetHistoryRequest : IRequest<CasinoHoldemPokerBetHistory>
    {
        public CasinoHoldemPokerBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
