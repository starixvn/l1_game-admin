using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoBetHistoryRequest : IRequest<KenoBetHistoryViewModel>
    {
        public GetKenoBetHistoryRequest(GameType game, long roundId, string nickname)
        {
            Game = game;
            RoundId = roundId;
            Nickname = nickname;
        }

        public GameType Game { get; }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
