
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class Win3CardsBetHistoryRequest : IRequest<Win3CardsBetHistoryRequest>
    {
        public Win3CardsBetHistoryRequest(long roundId, string userId)
        {
            RoundId = roundId;
            UserId = userId;
        }

        public long RoundId { get; }

        public string UserId { get; }
    }
}
