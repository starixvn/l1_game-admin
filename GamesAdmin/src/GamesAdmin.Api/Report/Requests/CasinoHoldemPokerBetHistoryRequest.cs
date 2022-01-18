using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CasinoHoldemPokerBetHistoryRequest : IRequest<CasinoHoldemPokerBetHistory>
    {
        public CasinoHoldemPokerBetHistoryRequest(CasinoHoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }


        public CasinoHoldemPokerBetHistoryFilter Filter { get; }
    }
}
