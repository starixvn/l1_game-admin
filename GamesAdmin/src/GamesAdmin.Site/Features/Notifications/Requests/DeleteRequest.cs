using MediatR;

namespace GamesAdmin.Site.Features.Notifications.Requests
{
    public class DeleteRequest : IRequest<bool>
    {
        public DeleteRequest(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
