using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Request
{
    public class BigSmallReportRequest : IRequest<IEnumerable<BigSmallReportRecord>>
    {
        public BigSmallReportRequest(int roundId, string nickname, bool excludeBot)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeBot = excludeBot;
        }

        public int RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeBot { get; }
    }
}