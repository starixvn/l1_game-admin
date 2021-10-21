using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateSuperPlayersRequest : IRequest<bool>
    {
        public string SuperPlayers { get; set; }
    }
}