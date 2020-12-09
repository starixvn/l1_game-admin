using System.Threading.Tasks;
using GamesAdmin.Site.Features.Bot.Requests;
using GamesAdmin.Site.Features.Bot.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Bot
{
    [Authorize]
    public class BotController : Controller
    {
        private readonly IMediator mediator;

        public BotController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            var resutls = await mediator.Send(new AddUsersRequest(addViewModel.TextNames));

            addViewModel.Results = resutls;

            return View(addViewModel);
        }

        [HttpGet]
        public IActionResult Deactivate() => View();

        [HttpPost]
        public async Task<IActionResult> Deactivate([Bind] DeactivateRequest request)
        => View(new DeactivateViewModel(await mediator.Send(request)));
    }
}
