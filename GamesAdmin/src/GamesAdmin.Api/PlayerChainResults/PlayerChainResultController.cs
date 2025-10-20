using System.Collections.Generic;
using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.PlayerChainResults.Requests;
using GamesAdmin.Api.PlayerChainResults.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.PlayerChainResults
{
    [Route("api/player-chain-results")]
    public class PlayerChainResultController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public PlayerChainResultController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerChainResults([FromQuery] string gameId = null, [FromQuery] string playerId = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        {
            var request = new GetPlayerChainResultsRequest
            {
                GameId = gameId,
                PlayerId = playerId,
                Page = page,
                PageSize = pageSize
            };

            var response = await mediator.Send(request);
            return Ok(response);
        }
    }
}
