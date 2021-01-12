using MediatR;

namespace GamesAdmin.Site.Features.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<bool>
    {
        public VoidTicketRequest(int memberId, string siteId, long gameRoundId, byte gameTypeId, string reason, bool freeBet)
        {
            ObCustId = memberId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Reason = reason;
            FreeBet = freeBet;
        }

        public int ObCustId { get; }

        public string SiteId { get; }

        public long GameRoundId { get; }

        public byte GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }
    }
}