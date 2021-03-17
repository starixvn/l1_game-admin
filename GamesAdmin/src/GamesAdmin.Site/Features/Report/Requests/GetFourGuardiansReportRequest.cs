using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetFourGuardiansReportRequest : IRequest<FourGuardiansReportResultViewModel>
    {
        public GetFourGuardiansReportRequest(long roundId, string nickname, bool excludeUUS)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}