using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models.BetReport;
using MediatR;

namespace GamesAdmin.Api.Report.Request
{
    public class KenoReportRequest : IRequest<KenoBetReport>
    {
        public KenoReportRequest(string game, long roundId, string nickname, bool excludeBot)
        {
            Game = Enumeration.FromValue<GameType>(game);
            RoundId = roundId;
            Nickname = nickname;
            ExcludeBot = excludeBot;
        }

        public GameType Game { get; }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeBot { get; }
    }
}
