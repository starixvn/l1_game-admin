
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class WinThreeCardsBetHistoryRequest : IRequest<WinThreeCardsBetHistoryRequest>
    {
        public WinThreeCardsBetHistoryRequest(long roundId, string userId)
        {
            RoundId = roundId;
            UserId = userId;
        }

        public long RoundId { get; }

        public string UserId { get; }
    }
}
