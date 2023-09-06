using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBlackjackReportRequest : IRequest<BankingGameReportResultViewModel>
    {
        public GetBlackjackReportRequest(BankingGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public BankingGameBetReportFilter Filter { get; }
    }
}