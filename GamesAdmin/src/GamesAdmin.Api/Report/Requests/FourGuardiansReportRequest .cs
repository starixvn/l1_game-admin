using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Report.Requests
{
    public class FourGuardiansReportRequest : IRequest<IEnumerable<FourGuardiansBetRecord>>
    {
        public FourGuardiansReportRequest(long roundId, string nickname, bool excludeUUS, string game)
        {
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
            Game = Enumeration.FromValue<Game>(game);
        }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }

        public Game Game { get; }
    }
}