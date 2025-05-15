using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CrashGameReportRequest : IRequest<CrashGameBetReport>
    {
        public CrashGameReportRequest(CrashGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public CrashGameBetReportFilter Filter { get; }
    }
}
