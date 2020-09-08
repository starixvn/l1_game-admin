using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class KenoBetHistoryRequest : IRequest<IEnumerable<KenoBetHistory>>
    {
        public KenoBetHistoryRequest(string game, long roundId, string nickname)
        {
            Game = Enumeration.FromValue<GameType>(game);
            RoundId = roundId;
            Nickname = nickname;
        }

        public GameType Game { get; }

        public long RoundId { get; }

        public string Nickname { get; }
    }
}
