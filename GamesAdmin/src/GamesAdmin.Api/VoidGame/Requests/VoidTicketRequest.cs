using GamesAdmin.Core.OWResults;
using MediatR;

namespace GamesAdmin.Api.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<VoidGameResult>
    {
        public VoidTicketRequest(string playerId, string operatorId, string siteId, int gameRoundId, int gameTypeId, string reason, bool freeBet)
        {
            PlayerId = playerId;
            OperatorId = operatorId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameTypeId = gameTypeId;
            Reason = reason;
            FreeBet = freeBet;
        }

        public string PlayerId { get; }

        public string OperatorId { get; }

        public string SiteId { get; }

        public long GameRoundId { get; set; }

        public int GameTypeId { get; }

        public string Reason { get; }

        public bool FreeBet { get; }
    }
}