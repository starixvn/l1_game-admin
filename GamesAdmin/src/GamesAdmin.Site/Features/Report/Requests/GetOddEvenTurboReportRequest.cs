using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetOddEvenTurboReportRequest : IRequest<OddEvenTurboReportResultViewModel>
    {
        public GetOddEvenTurboReportRequest(long roundId, string nickname, bool excludeBot)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeBot = excludeBot;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeBot { get; }
    }
}