using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class BetHistoryReportRequest : IRequest<BetHistoryViewModel>
    {
        public BetHistoryReportRequest(BetHistoryFilter filter, bool convertToUSD)
        {
            Filter = filter;
            ConvertToUSD = convertToUSD;
        }

        public BetHistoryFilter Filter { get; }
        public bool ConvertToUSD { get; }
    }
}