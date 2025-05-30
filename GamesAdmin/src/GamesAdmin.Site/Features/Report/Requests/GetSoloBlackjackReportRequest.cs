using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSoloBlackjackReportRequest : IRequest<BlackjackGameReportResultViewModel>
    {
        public GetSoloBlackjackReportRequest(BlackjackBetReportFilter filter)
        {
            Filter = filter;
        }

        public BlackjackBetReportFilter Filter { get; }
    }
}
