using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class KenoMatchReportRequest : IRequest<KenoMatchBetReport>
    {
        public KenoMatchReportRequest(GameId game, long roundId, string nickname, bool excludeUUS)
        {
            Game = game;
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }


        public GameId Game { get; }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}