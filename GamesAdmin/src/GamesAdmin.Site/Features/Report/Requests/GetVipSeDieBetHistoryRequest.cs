using System.Collections.Generic;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetVipSeDieBetHistoryRequest : IRequest<IEnumerable<VipSeDieBetHistoryRecordViewModel>>
    {
        public GetVipSeDieBetHistoryRequest(long roundId, string userId)
        {
            RoundId = roundId;
            UserId = userId;
        }

        public long RoundId { get; }

        public string UserId { get; }
    }
}