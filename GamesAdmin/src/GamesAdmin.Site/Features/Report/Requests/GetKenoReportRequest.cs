using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoReportRequest : IRequest<KenoReportViewModel>
    {
        public GetKenoReportRequest(
            Game game, 
            long roundId, 
            string nickname, 
            bool excludeUUS = true)
        {
            Game = game;
            RoundId = roundId;
            Nickname = nickname;
            ExcludeUUS = excludeUUS;
        }

        public Game Game { get; }

        public long RoundId { get; }

        public string Nickname { get; }

        public bool ExcludeUUS { get; }
    }
}