using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBlackjackReportRequest : IRequest<BlackjackReportResultViewModel>
    {
        public GetBlackjackReportRequest(long handId, string nickname, bool excludeUUS)
        {
            HandId = handId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public long HandId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}