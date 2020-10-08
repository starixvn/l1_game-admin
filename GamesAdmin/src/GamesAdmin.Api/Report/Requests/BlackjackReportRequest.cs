using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class BlackjackReportRequest : IRequest<IEnumerable<BlackjackBetRecord>>
    {
        public BlackjackReportRequest(long handId, string nickname, bool excludeUUS)
        {
            HandId = handId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public long HandId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}