using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;

namespace GamesAdmin.Api.Report.Requests
{
    public class PontoonReportRequest : IRequest<IEnumerable<BankingGameBetRecord>>
    {
        public PontoonReportRequest(BankingGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public BankingGameBetReportFilter Filter { get; }
    }
}
