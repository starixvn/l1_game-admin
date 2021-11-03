using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoPowerBetHistoryRequest : IRequest<KenoPowerBetHistoryViewModel>
    {
        public GetKenoPowerBetHistoryRequest(GameId game, long roundId, string userId)
        {
            Game = game;
            RoundId = roundId;
            UserId = userId;
        }

        public GameId Game { get; }

        public long RoundId { get; }

        public string UserId { get; }
    }
}