using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetHiLoReportRequest : IRequest<HiLoReportResultViewModel>
    {
        public GetHiLoReportRequest(HiLoBetReportFilter filter)
        {
            Filter = filter;
        }

        public HiLoBetReportFilter Filter { get; }
    }
}