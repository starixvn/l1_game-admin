using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetJWTRequest : IRequest<GameServerToken>
    {
        public string UserAgent { get; set; }

        public string Username { get; set; }

        public string Game { get; set; }
    }
}
