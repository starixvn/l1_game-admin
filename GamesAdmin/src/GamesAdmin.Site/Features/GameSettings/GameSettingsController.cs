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
        private readonly IMediator mediator;

        public GameSettingsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Games()
        => View(await mediator.Send(new GamesRequest()));

        [HttpPost]  
        [Route("{id}")]
        public async Task<IActionResult> ToogleOnOff(string id)
        {
            await mediator.Send(new ToogleOnOffRequest(id));

            return RedirectToAction(nameof(Games));
        }
    }
}