using System.Collections.Generic;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetOddEvenBetHistoryRequest : IRequest<IEnumerable<OddEvenBetHistoryViewModel>>
    {
        public GetOddEvenBetHistoryRequest(long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
        }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}