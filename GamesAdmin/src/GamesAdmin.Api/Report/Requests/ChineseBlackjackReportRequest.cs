using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class ChineseBlackjackReportRequest : IRequest<IEnumerable<BlackjackBetRecord>>
    {
        public ChineseBlackjackReportRequest(BlackjackBetReportFilter filter)
        {
            Filter = filter;
        }

        public BlackjackBetReportFilter Filter { get; }
    }
}
