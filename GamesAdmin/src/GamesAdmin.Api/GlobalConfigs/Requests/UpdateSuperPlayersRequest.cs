using MediatR;

namespace GamesAdmin.Api.GlobalConfigs.Requests
{
    public class UpdateSuperPlayersRequest : IRequest<bool>
    {
        public UpdateSuperPlayersRequest(string[] superPlayers)
        {
            SuperPlayers = superPlayers;
        }

        public string[] SuperPlayers { get; set; }
    }
}