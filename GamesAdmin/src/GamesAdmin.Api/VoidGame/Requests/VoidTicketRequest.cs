using GamesAdmin.Core.OWResults;
using MediatR;

namespace GamesAdmin.Api.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<VoidGameResult>
    {
        public VoidTicketRequest(int obCustId, string partnerId, string siteId, int gameRoundId, byte gameTypeId, string reason, bool freeBet)
        {
            PlayerId = obCustId;
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

        public byte GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }
    }
}