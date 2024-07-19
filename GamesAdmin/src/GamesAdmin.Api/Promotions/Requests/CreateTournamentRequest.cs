using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class CreateTournamentRequest : IRequest<bool>
    {
        public CreateTournamentRequest(PromotionConfigModel model)
        {
            Model = model;
        }

        public PromotionConfigModel Model { get; }
    }
}
