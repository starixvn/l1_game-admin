using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.RoundResults;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class KenoRoundResultRequest : IRequest<KenoRoundResult>
    {
        public KenoRoundResultRequest(string game, long roundId)
        {
            Game = Enumeration.FromValue<GameType>(game);
            RoundId = roundId;
        }

        public GameType Game { get; }

        public long RoundId { get; }
    }
}
