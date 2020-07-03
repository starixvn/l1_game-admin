using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Request
{
    public class KenoReportRequest : IRequest<IEnumerable<KenoBetRecord>>
    {
        public KenoReportRequest(string game, long roundId)
        {
            Game = Enumeration.FromValue<GameType>(game);
            RoundId = roundId;
        }

        public GameType Game { get; }

        public long RoundId { get; }
    }
}
