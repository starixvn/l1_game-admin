using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameServers.Requests;
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
        public async Task<bool> Reload()
           => (await mediator.Send(new ReloadRequest()));

        [HttpPost("jwt")]
        public async Task<JsonWebToken> GetJWT([FromBody] JWTRequest request)
           => (await mediator.Send(request));
    }
}
