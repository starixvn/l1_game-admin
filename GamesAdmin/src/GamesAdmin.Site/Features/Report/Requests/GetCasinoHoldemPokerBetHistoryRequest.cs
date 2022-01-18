using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCasinoHoldemPokerBetHistoryRequest : IRequest<CasinoHoldemPokerBetHistory>
    {
        public GetCasinoHoldemPokerBetHistoryRequest(CasinoHoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }

        public CasinoHoldemPokerBetHistoryFilter Filter { get; }
    }
}
