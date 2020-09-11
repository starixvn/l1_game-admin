using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.GameServers.Requests;
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
    }
}
