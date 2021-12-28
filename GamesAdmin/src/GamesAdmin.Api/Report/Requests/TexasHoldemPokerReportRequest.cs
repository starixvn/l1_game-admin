using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class TexasHoldemPokerReportRequest : IRequest<IEnumerable<TexasHoldemPokerBetRecord>>
    {
        public TexasHoldemPokerReportRequest(TexasHoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public TexasHoldemPokerBetReportFilter Filter { get; }
    }
}