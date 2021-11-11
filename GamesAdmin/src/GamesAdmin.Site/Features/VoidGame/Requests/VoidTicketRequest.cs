using GamesAdmin.Core.OWResults;
using MediatR;

namespace GamesAdmin.Site.Features.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<VoidGameResult>
    {
        public VoidTicketRequest(int memberId, string partnerId, string siteId, long gameRoundId, byte gameTypeId, string reason, bool freeBet)
        {
            ObCustId = memberId;
            PartnerId = partnerId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Reason = reason;
            FreeBet = freeBet;
        }

        public int ObCustId { get; }

        public string PartnerId { get; }

        public string SiteId { get; }

        public long GameRoundId { get; }

        public byte GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }
    }
}