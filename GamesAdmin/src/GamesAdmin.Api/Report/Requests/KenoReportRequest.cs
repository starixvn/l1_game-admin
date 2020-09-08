using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class KenoReportRequest : IRequest<IEnumerable<KenoBetRecord>>
    {
        public KenoReportRequest(
            string game, 
            long roundId,
            string nickname,
            bool excludeUUS)
        {
            Game = Enumeration.FromValue<GameType>(game);
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public GameType Game { get; }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}
