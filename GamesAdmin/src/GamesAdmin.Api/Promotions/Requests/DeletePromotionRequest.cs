using MediatR;

namespace GamesAdmin.Api.Promotions.Requests
{
    public class DeletePromotionRequest : IRequest<bool>
    {
        public DeletePromotionRequest(string id)
        {
            Id = id;
        }
        public string Id { get; }
    }
}
