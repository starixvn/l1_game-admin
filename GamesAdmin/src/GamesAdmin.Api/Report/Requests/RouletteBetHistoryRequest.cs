using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class RouletteBetHistoryRequest : IRequest<RouletteBetHistory>
    {
        public RouletteBetHistoryRequest(long roundId, string userId)
        {
            RoundId = roundId;
            UserId = userId;
        }

        public long RoundId { get; }

        public string UserId { get; }
    }
}