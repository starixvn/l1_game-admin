using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class AwardTournamentRequest : IRequest<bool>
    {
        public AwardTournamentRequest(int promotionId)
        {
            PromotionId = promotionId;
        }

        public int PromotionId { get; }
    }
}