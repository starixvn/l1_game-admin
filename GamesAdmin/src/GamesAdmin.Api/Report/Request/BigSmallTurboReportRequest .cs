using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Request
{
    public class BigSmallTurboReportRequest : IRequest<BigSmallBetReport>
    {
        public BigSmallTurboReportRequest(long roundId, string nickname, bool excludeBot)
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