using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.ProVietKeno.Requests
{
    public class GetByRoundIdRequest : IRequest<ProVietKenoRound>
    {
        public GetByRoundIdRequest(long roundId)
        {
            RoundId = roundId;
        }

        public long RoundId { get; }
    }
}