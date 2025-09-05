using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class KenoMatchBetHistoryRequest : IRequest<KenoMatchBetHistory>
    {
        public KenoMatchBetHistoryRequest(GameId game, long roundId, string userId)
        {
            Game = (int)game;
            RoundId = roundId;
            UserId = userId;
        }

        public int Game { get; set; }

        public long RoundId { get; }

        public string UserId { get; }
    }
}