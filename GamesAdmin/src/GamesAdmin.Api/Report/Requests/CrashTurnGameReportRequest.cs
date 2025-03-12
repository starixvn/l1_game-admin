using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class CrashTurnGameReportRequest : IRequest<IEnumerable<CrashTurnGameBetRecord>>
    {
        public CrashTurnGameReportRequest(CrashTurnGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public CrashTurnGameBetReportFilter Filter { get; }
    }
}
