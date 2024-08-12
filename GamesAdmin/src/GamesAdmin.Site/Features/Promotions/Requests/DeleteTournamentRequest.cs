using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class DeleteTournamentRequest : IRequest<bool>
    {
        public DeleteTournamentRequest(string id)
        {
            Id = id;
        }

        public string Id{ get; }
    }
}
