using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class BetReportRequest : IRequest<BetReportBase>
    {
        public BetReportRequest(BetReportFilter filter)
        {
            Filter = filter;
        }

        public BetReportFilter Filter { get; }
    }
}