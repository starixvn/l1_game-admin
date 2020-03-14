using System.Threading.Tasks;
using GamesAdmin.Site.Features.GameSettings.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.GameSettings
{
    [Authorize]
    public class GameSettingsController : Controller
    {
        protected readonly IMediator mediator;

        public GameSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Games()
        => View(await mediator.Send(new GamesRequest()));

        [HttpPost]  
        [Route("{id}")]
        public async Task<IActionResult> Enable(string id)
        {
            await mediator.Send(new UpdateEnableRequest(id, true));

            return RedirectToAction(nameof(Games));
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Disable(string id)
        {
            await mediator.Send(new UpdateEnableRequest(id, false));

            return RedirectToAction(nameof(Games));
        }
    }
}