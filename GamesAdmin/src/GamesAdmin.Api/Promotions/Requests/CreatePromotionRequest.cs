using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class CreatePromotionRequest : IRequest<bool>
    {
        public CreatePromotionRequest(PromotionModel model)
        {
            Model = model;
        }

        public PromotionModel Model { get; }
    }
}
