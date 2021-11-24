using GamesAdmin.Core.Models.BetReport;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class ReportRequest
    {
        public ReportRequest(BetReportFilter filter)
        {
            Filter = filter;
        }

        public BetReportFilter Filter { get; }
    }
}