using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetTexasHoldemPokerReportRequest : IRequest<TexasHoldemPokerReportResultViewModel>
    {
        public GetTexasHoldemPokerReportRequest(TexasHoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public TexasHoldemPokerBetReportFilter Filter { get; }
    }
}