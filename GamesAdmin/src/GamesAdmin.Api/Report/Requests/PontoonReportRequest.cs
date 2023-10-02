using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class PontoonReportRequest : IRequest<IEnumerable<BlackjackBetRecord>>
    {
        public PontoonReportRequest(BlackjackBetReportFilter filter)
        {
            Filter = filter;
        }

        public BlackjackBetReportFilter Filter { get; }
    }
}
