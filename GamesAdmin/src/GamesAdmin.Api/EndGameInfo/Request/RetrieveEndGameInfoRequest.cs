using GamesAdmin.Core.Models.EndGameInfo;
using MediatR;

namespace GamesAdmin.Api.EndGameInfo.Request
{
    public class RetrieveEndGameInfoRequest : IRequest<EndGameInfoResult>
    {
        public int GameRoundId { get; set; }

        public byte GameTypeId { get; set; }

        public int ObCustId { get; set; }

        public string SiteId { get; set; }

        public bool FreeBet { get; set; }

        public string PartnerId { get; set; }
    }
}