using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Report.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Report.Requests
{
    public class HoldemPokerReportRequest : IRequest<HoldemPokerReportViewModel>
    {
        public HoldemPokerReportRequest(GameId gameId)
        {
            GameId = (int)gameId;
        }

        public int GameId { get; }
    }
}