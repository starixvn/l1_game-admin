using System.Collections.Generic;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class VipSeDieBetHistoryRequest : IRequest<IEnumerable<VipSeDieBetHistoryRecord>>
    {
        public VipSeDieBetHistoryRequest(long roundId, string userId)
        {
            RoundId = roundId;
            UserId = userId;
        }

        public long RoundId { get; }

        public string UserId { get; }
    }
}