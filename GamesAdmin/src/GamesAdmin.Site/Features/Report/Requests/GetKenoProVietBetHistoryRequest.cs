using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoProVietBetHistoryRequest : IRequest<KenoBetHistoryViewModel>
    {
        public GetKenoProVietBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
