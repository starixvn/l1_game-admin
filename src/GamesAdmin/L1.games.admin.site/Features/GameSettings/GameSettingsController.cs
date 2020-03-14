using System.Threading.Tasks;
using L1.games.admin.site.Features.GameSettings.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L1.games.admin.site.Features.GameSettings
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
    }
}