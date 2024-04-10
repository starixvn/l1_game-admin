using GamesAdmin.Site.Features.VoidGame.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<VoidGameResultViewModel>
    {
        public VoidTicketRequest(string memberId, string partnerId, string siteId, long gameRoundId, int gameTypeId, string reason, bool freeBet, bool callApi = true)
        {
            PlayerId = memberId;
            OperatorId = partnerId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Reason = reason;
            FreeBet = freeBet;
            CallApi = callApi;
        }

        public string PlayerId { get; }

        public string OperatorId { get; }

        public string SiteId { get; }

        public long GameRoundId { get; }

        public int GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }

        public bool CallApi { get; }
    }
}