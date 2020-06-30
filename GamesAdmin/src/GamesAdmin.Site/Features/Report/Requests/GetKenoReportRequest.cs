using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class GetKenoReportRequest : IRequest<KenoReportViewModel>
    {
        public GetKenoReportRequest(GameType game, long roundId)
        {
            Game = game;
            RoundId = roundId;
        }

        public GameType Game { get; }

        public long RoundId { get; }
    }
}