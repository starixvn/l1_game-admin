using GamesAdmin.Core.Models.BetReport;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetSlotRNGReportRequest : IRequest<SlotRNGReportResultViewModel>
    {
        public GetSlotRNGReportRequest(BetReportFilter filter)
        {
            Filter = filter;
        }

        public BetReportFilter Filter { get; }
    }
}