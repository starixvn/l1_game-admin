using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetBetHistoryRequest : IRequest<IEnumerable<BaseBetHistory>>
    {
        public GetBetHistoryRequest(byte gameTypeId, long roundId, string nickname)
        {
            RoundId = roundId;
            Nickname = nickname;
            GameTypeId = gameTypeId;
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public byte GameTypeId { get; }
    }
}