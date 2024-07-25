using System.Collections.Generic;
using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetTournamentWinningPlayersRequest : IRequest<IEnumerable<AwardWinningPlayer>>
    {
        public GetTournamentWinningPlayersRequest(int promotionId)
        {
            this.PromotionId = promotionId;
        }

        public int PromotionId { get; set; }
    }
}