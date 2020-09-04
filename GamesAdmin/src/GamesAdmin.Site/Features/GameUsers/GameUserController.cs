using GamesAdmin.Site.Features.GameUsers.Requests;
using GamesAdmin.Site.Features.GameUsers.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.GameUsers
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

        [HttpGet]
        public IActionResult Spins()
        => View(new GetSpinViewModel());

        [HttpGet]
        public async Task<IActionResult> GetMemberSpins(int memberId)
        => PartialView("_SpinRecords", await mediator.Send(new GetSpinRequest(memberId)));

        [HttpGet]
        public IActionResult UpdateSpin(int memberId)
        => View(new UpdateSpinViewModel(memberId));
    }
}
