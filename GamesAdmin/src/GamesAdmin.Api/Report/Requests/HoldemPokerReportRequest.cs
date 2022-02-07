using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class HoldemPokerReportRequest : IRequest<IEnumerable<HoldemPokerBetRecord>>
    {
        public HoldemPokerReportRequest(HoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public HoldemPokerBetReportFilter Filter { get; }
    }
}