using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetCasinoHoldemPokerReportRequest : IRequest<CasinoHoldemPokerReportResultViewModel>
    {
        public GetCasinoHoldemPokerReportRequest(CasinoHoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public CasinoHoldemPokerBetReportFilter Filter { get; }
    }
}