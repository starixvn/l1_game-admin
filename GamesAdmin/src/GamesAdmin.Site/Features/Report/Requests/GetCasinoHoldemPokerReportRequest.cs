using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCasinoHoldemPokerReportRequest : IRequest<CasinoHoldemPokerReportResultViewModel>
    {
        public GetCasinoHoldemPokerReportRequest(HoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public HoldemPokerBetReportFilter Filter { get; }
    }
}