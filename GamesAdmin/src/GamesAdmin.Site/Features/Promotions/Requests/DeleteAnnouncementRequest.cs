using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class DeleteAnnouncementRequest : IRequest<bool>
    {
        public DeleteAnnouncementRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
