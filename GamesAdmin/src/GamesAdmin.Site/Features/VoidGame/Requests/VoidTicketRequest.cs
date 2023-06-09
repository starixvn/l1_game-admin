using GamesAdmin.Core.OWResults;
using GamesAdmin.Site.Features.VoidGame.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<VoidGameResultViewModel>
    {
        public VoidTicketRequest(int memberId, string partnerId, string siteId, long gameRoundId, int gameTypeId, string reason, bool freeBet)
        {
            PlayerId = memberId;
            OperatorId = partnerId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Reason = reason;
            FreeBet = freeBet;
        }

        public int PlayerId { get; }

        public string OperatorId { get; }

        public string SiteId { get; }

        public long GameRoundId { get; }

        public int GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }
    }
}