using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.EndGameInfo.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.EndGameInfo
{
    [Route("api/endgameinfo")]
    public class EndGameInfoController : BaseAuthorizeController
    {
        private IMediator mediator;

        public EndGameInfoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] RetrieveEndGameInfoRequest request)
            => Ok(new { summary = await mediator.Send(request) });
    }
}