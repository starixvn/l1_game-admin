using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetHoldemPokerBetHistoryRequest : IRequest<HoldemPokerBetHistoryViewModel>
    {
        public GetHoldemPokerBetHistoryRequest(HoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }

        public HoldemPokerBetHistoryFilter Filter { get; }
    }
}
