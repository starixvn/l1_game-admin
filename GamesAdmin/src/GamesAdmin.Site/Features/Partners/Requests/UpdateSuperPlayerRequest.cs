using MediatR;

namespace GamesAdmin.Site.Features.Partners.Requests
{
    public class UpdateSuperPlayerRequest : IRequest<bool>
    {
        public UpdateSuperPlayerRequest(string partnerId, string[] superPlayers)
        {
            PartnerId = partnerId;
            SuperPlayers = superPlayers;
        }

        public string PartnerId { get; }

        public string[] SuperPlayers { get; set; }
    }
}
