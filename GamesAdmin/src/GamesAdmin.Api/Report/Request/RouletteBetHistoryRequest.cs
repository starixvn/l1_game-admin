using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Request
{
    public class RouletteBetHistoryRequest : IRequest<RouletteBetHistory>
    {
        public RouletteBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}