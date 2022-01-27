using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class TexasHoldemPokerBetHistoryRequest : IRequest<TexasHoldemPokerBetHistory>
    {
        public TexasHoldemPokerBetHistoryRequest(HoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }


        public HoldemPokerBetHistoryFilter Filter { get; }
    }
}
