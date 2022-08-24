using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class GetPromotionByIdRequest : IRequest<PromotionModel>
    {
        public GetPromotionByIdRequest(string id)
        {
            Id = id;
        }
        public string Id { get; }
    }
}
