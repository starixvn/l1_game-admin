using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoProVietReportRequest : IRequest<KenoReportViewModel>
    {
        public GetKenoProVietReportRequest(long roundId)
        {
            RoundId = roundId;
        }

        public long RoundId { get; }
    }
}