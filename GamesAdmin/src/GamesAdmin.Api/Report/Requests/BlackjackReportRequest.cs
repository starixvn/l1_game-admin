using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class BlackjackReportRequest : IRequest<IEnumerable<BlackjackBetRecord>>
    {
        public BlackjackReportRequest(BlackjackBetReportFilter filter)
        {
            Filter = filter;
        }

        public BlackjackBetReportFilter Filter { get; }
    }
}