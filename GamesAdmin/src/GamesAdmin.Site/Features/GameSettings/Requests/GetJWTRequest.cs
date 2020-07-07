using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetJWTRequest : IRequest<JWTFromGameServer>
    {
        public string UserAgent { get; set; }

        public string Username { get; set; }
    }
}
