using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCasinoHoldemPokerBetHistoryRequest : IRequest<CasinoHoldemPokerBetHistoryViewModel>
    {
        public GetCasinoHoldemPokerBetHistoryRequest(CasinoHoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }

        public CasinoHoldemPokerBetHistoryFilter Filter { get; }
    }
}
