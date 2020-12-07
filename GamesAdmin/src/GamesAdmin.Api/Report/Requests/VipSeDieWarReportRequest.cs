using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class VipSeDieWarReportRequest : IRequest<VipSeDieWarBetReport>
    {
        public VipSeDieWarReportRequest(long roundId, string nickname, bool excludeUUS)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}