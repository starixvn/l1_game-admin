using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class BlackjackReportRequest : IRequest<IEnumerable<BankingGameBetRecord>>
    {
        public BlackjackReportRequest(BankingGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public BankingGameBetReportFilter Filter { get; }
    }
}