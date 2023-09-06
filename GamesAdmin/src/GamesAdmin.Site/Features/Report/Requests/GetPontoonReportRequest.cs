using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetPontoonReportRequest : IRequest<BankingGameReportResultViewModel>
    {
        public GetPontoonReportRequest(BankingGameBetReportFilter filter)
        {
            Filter = filter;
        }

        public BankingGameBetReportFilter Filter { get; }
    }
}
