namespace GamesAdmin.Api.ViewPlayerInfo
{
    using GamesAdmin.Api._Shared;
    using GamesAdmin.Api.PlayerInfo.Requests;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    [Route("api/playerInfo")]
    public class PlayerInfoController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public PlayerInfoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("get")]
        public async Task<IActionResult> Get(GetPlayerInfoRequest request)
        {
            var playerInfos = await mediator.Send(request);

            return Ok(playerInfos);
        }    
    }
}
