using GamesAdmin.Core.Models.Promotion;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class UpdateTournamentRequest : IRequest<bool>
    {
        public UpdateTournamentRequest(PromotionConfigModel model)
        {
            Model = model;
        }

        public PromotionConfigModel Model { get; }
    }
}
