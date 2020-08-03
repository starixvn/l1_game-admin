using GamesAdmin.Site.Features.GameUser.Requests;
using GamesAdmin.Site.Features.GameUser.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameUser
{
    [Authorize]
    public class GameUserController : Controller
    {
        private readonly IMediator mediator;

        public GameUserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));

        [HttpPost]
        public async Task<IActionResult> Add([Bind] AddViewModel addViewModel)
        {
            var resutls = await mediator.Send(new AddUsersRequest(addViewModel.TextNames, addViewModel.IsBot));

            addViewModel.Results = resutls;

            return View(addViewModel);
        }

        [HttpGet]
        public IActionResult ReviseBots()
        => View(new ReviseBotViewModel());

        [HttpPost]
        public async Task<IActionResult> ReviseBots([Bind] ReviseBotViewModel reviseViewModel)
        {
            reviseViewModel.Updated = await mediator.Send(new ReviseRequest());
            reviseViewModel.Success = true;

            return View(reviseViewModel);
        }
    }
}
