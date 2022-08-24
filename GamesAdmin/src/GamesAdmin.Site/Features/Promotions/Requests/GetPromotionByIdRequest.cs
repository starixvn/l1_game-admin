using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class GetPromotionByIdRequest : IRequest<UpdatePromotionViewModel>
    {
        public GetPromotionByIdRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
