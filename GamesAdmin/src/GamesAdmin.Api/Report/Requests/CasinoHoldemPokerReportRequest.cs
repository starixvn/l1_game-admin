using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class CasinoHoldemPokerReportRequest : IRequest<IEnumerable<CasinoHoldemPokerBetRecord>>
    {
        public CasinoHoldemPokerReportRequest(CasinoHoldemPokerBetReportFilter filter)
        {
            Filter = filter;
        }

        public CasinoHoldemPokerBetReportFilter Filter { get; }
    }
}