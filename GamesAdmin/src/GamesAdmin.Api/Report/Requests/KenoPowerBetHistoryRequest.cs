using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class KenoPowerBetHistoryRequest : IRequest<KenoPowerBetHistory>
    {
        public KenoPowerBetHistoryRequest(GameId game, long roundId, string userId)
        {
            Game = game;
            RoundId = roundId;
            UserId = userId;
        }

        public GameId Game { get; set; }

        public long RoundId { get; }

        public string UserId { get; }
    }
}