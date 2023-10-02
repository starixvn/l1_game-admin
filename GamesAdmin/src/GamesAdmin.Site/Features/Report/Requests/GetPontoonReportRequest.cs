using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetPontoonReportRequest : IRequest<BlackjackGameReportResultViewModel>
    {
        public GetPontoonReportRequest(BlackjackBetReportFilter filter)
        {
            Filter = filter;
        }

        public BlackjackBetReportFilter Filter { get; }
    }
}
