using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetTexasHoldemPokerBetHistoryRequest : IRequest<TexasHoldemPokerBetHistoryViewModel>
    {
        public GetTexasHoldemPokerBetHistoryRequest(HoldemPokerBetHistoryFilter filter)
        {
            Filter = filter;
        }

        public HoldemPokerBetHistoryFilter Filter { get; }
    }
}
