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
        public IActionResult ReviseBots()
        => View(new ReviseBotViewModel());

        [HttpGet]
        public IActionResult Spins()
        => View(new GetSpinViewModel());

        [HttpGet]
        public async Task<IActionResult> GetMemberSpins(int memberId)
        => PartialView("_SpinRecords", await mediator.Send(new GetSpinRequest(memberId)));     
    }
}
