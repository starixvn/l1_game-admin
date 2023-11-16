using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CrashGameReportRequest : IRequest<CrashGameBetReport>
    {
        public CrashGameReportRequest(SpeedUpGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public SpeedUpGameBetReportFilter Filter { get; }
    }
}
