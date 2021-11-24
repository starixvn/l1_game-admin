using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSeDieReportRequest : IRequest<SeDieReportResultViewModel>
    {
        public GetSeDieReportRequest(BetReportFilter filter)
        {
            Filter = filter;
        }

        public BetReportFilter Filter { get; }
    }
}