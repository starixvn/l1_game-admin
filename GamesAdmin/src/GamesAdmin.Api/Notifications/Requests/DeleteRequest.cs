using MediatR;

namespace GamesAdmin.Api.Notifications.Requests
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
