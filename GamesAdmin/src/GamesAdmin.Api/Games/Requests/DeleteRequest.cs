using MediatR;

namespace GamesAdmin.Api.Games.Requests
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
