using MediatR;

namespace GamesAdmin.Api.Partners.Requests
{
    public class UpdateSuperPlayersRequest : IRequest<bool>
    {
        public UpdateSuperPlayersRequest() { }

        public UpdateSuperPlayersRequest(string partnerId, string[] superPlayers)
        {
            PartnerId = partnerId;
            SuperPlayers = superPlayers;
        }

        public string PartnerId { get; set; }

        public string[] SuperPlayers { get; set; }
    }
}