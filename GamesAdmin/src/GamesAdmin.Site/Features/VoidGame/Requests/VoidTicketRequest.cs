using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.VoidGame.Requests
{
    public class VoidTicketRequest : IRequest<bool>
    {
        public VoidTicketRequest(int memberId, int siteId, long gameRoundId, GameId gameId, string reason)
        {
            MemberId = memberId;
            SiteId = siteId;
            GameRoundId = gameRoundId;
            GameId = gameId;
            Reason = reason;
        }

        public int MemberId { get; }

        public int SiteId { get; }

        public long GameRoundId { get; }

        public GameId GameId { get; }

        public string Reason { get; }
    }
}