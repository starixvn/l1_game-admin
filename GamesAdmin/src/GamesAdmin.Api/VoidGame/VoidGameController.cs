using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.VoidGame.Requests;
using GamesAdmin.Core.OWResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.VoidGame
{
    [Route("api/void")]
    public class VoidGameController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public VoidGameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<VoidGameResult> VoidTicket([FromBody] VoidTicketRequest request)
           => (await mediator.Send(request));
    }
}