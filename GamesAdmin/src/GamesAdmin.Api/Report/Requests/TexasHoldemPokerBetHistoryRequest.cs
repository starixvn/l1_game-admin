using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class TexasHoldemPokerBetHistoryRequest : IRequest<TexasHoldemPokerBetHistory>
    {
        public TexasHoldemPokerBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
