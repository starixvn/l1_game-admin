using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class JWTRequest : IRequest<JsonWebToken>
    {
        public string UserAgent { get; set; }

        public string Username { get; set; }
    }
}
