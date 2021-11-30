using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSeDieReportRequest : ReportRequest, IRequest<SeDieReportResultViewModel>
    {
        public GetSeDieReportRequest(BetReportFilter filter) : base( Core.Enumeration.GameId.SeDie ,filter)
        {
        }
    }
}