using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.ChatBot.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.ChatBot
{
    [Route("api/chatbot")]

    public class ChatBotController : BaseAuthorizeController
    {
        private readonly IMediator mediator;

        public ChatBotController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("notify_high_stake")]
        public async Task<IActionResult> NotifyHighStake([FromBody] NotifyHighStakeRequest request)
            => Ok(await mediator.Send(request));
    }
}