using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class UpdatePromotionRequest : IRequest<bool>
    {
        public UpdatePromotionRequest(PromotionModel model)
        {
            Model = model;
        }

        public PromotionModel Model { get; }
    }
}
