using MediatR;

namespace GamesAdmin.Site.Features.ProVietKeno.Requests
{
    public class SettleRoundRequest : IRequest
    {
        public SettleRoundRequest(long roundId)
        {
            RoundId = roundId;
        }

        public long RoundId { get; }
    }
}