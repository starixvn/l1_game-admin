using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class UpdatePromotionRequest : IRequest<MessageResult>
    {
        public UpdatePromotionRequest(PromotionViewModel model)
        {
            Model = model;
        }

        public PromotionViewModel Model { get; }
    }
}
