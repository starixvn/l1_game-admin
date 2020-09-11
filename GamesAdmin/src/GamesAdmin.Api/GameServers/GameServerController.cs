using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameServers.Requests;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.GameServers
{
    [Route("api/game_server")]
    public class GameServerController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public GameServerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("reload")]
        public Task<bool> Reload()
           => mediator.Send(new ReloadRequest());

        [HttpPost("jwt")]
        public Task<JsonWebToken> GetJWT([FromBody] JWTRequest request)
           => mediator.Send(request);

        [HttpGet("online_users/{game?}")]
        public Task<OnlineUsers> OnlineUsers(string game)
           => mediator.Send(new OnlineUserRequest(game));
    }
}
