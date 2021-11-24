using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetMiniSeDieReportRequest : ReportRequest, IRequest<SeDieReportResultViewModel>
    {
        public GetMiniSeDieReportRequest(BetReportFilter filter) : base(filter)
        {
        }
    }
}