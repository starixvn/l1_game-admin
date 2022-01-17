using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.ResultMonitor.Requests;
using GamesAdmin.Site.Features.ResultMonitor.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.ResultMonitor
{
    [Authorize(Roles = Role.AdminValue)]
    public class ResultMonitorController : Controller
    {
        private readonly IMediator mediator;

        public ResultMonitorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public IActionResult SlotResult(string gameConfigName)
            => View(new SlotResultViewModel(gameConfigName));

        [HttpPost]
        public async Task<IActionResult> SetSlotResult([Bind] SlotResultViewModel viewModel)
        {
            var viewName = viewModel.GameName == Game.HeroesSlotValue ?
                "_HeroesSlotResultMonitor" : "_ResultMonitor";
            return PartialView(viewName, await mediator.Send(new SetSlotResultRequest(
                viewModel.GameName,
                viewModel.Nickname,
                viewModel.Results.Trim().Split(',').Select(x => byte.Parse(x)).ToArray())));
        }

        [HttpGet]
        public IActionResult CasinoHoldemPokderResult()
           => View();

        [HttpPost]
        public async Task<IActionResult> SetCasinoHoldemPokerResult(SetCasinoHoldemPokerResultRequest request)
        {
            return PartialView("_CasinoHoldemPokderResult", await mediator.Send(request));
        }

        [HttpGet]
        public IActionResult HiLo()
        => View(new HiLoResultViewModel());

        [HttpPost]
        public async Task<IActionResult> SetHiLoResult(HiLoResultViewModel viewModel)
        {
            return PartialView("_HiLoResult", await mediator.Send(new SetHiLoResultRequest(viewModel.Nickname, viewModel.Result.Split(";"))));
        }
    }
}