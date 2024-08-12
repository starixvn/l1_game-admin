using System.Collections.Generic;
using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetTournamentWinningPlayersRequest : IRequest<IEnumerable<AwardWinningPlayer>>
    {
        public GetTournamentWinningPlayersRequest(int promotionId)
        {
            PromotionId = promotionId;
        }

        public int PromotionId { get; }
    }
}