using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class OddEvenTurboReportRequest : IRequest<OddEvenBetReport>
    {
        public OddEvenTurboReportRequest(long roundId, string nickname, bool excludeBot)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeBot = excludeBot;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeBot { get; }
    }
}