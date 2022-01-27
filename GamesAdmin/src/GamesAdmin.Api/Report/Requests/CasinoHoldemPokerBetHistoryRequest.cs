using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CasinoHoldemPokerBetHistoryRequest : IRequest<CasinoHoldemPokerBetHistory>
    {
        public CasinoHoldemPokerBetHistoryRequest(HoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }


        public HoldemPokerBetHistoryFilter Filter { get; }
    }
}
