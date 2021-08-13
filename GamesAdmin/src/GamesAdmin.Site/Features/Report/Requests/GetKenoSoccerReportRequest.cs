using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoSoccerReportRequest : IRequest<KenoMatchReportResultViewModel>
    {
        public GetKenoSoccerReportRequest(GameId game, long roundId, string nickname, bool excludeUUS)
        {
            Game = game;
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public GameId Game { get; set; }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}