using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class CreatePromotionRequest : IRequest<MessageResult>
    {
        public CreatePromotionRequest(CreatePromotionViewModel model)
        {
            Model = model;
        }

        public CreatePromotionViewModel Model { get; }
    }

}
