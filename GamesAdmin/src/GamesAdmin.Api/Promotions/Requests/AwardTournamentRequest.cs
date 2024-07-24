using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class AwardTournamentRequest : IRequest<bool>
    {
        public int PromotionId { get; set; }
    }
}