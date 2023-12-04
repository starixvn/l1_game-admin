using System.Threading.Tasks;
using Audit.Mvc;
using GamesAdmin.Site.Features.VoidGame.Requests;
using GamesAdmin.Site.Features.VoidGame.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.VoidGame
{
    [Authorize]
    public class VoidGameController : Controller
    {
        private readonly IMediator mediator;

        public VoidGameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await mediator.Send(new IndexViewRequest()));

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> VoidTicket([Bind] IndexViewModel viewModel)
        {
            var resultModel = await mediator.Send(new VoidTicketRequest(
                viewModel.MemberId,
                viewModel.PartnerId,
                viewModel.SiteId,
                viewModel.GameRoundId,
                viewModel.GameId,
                viewModel.Reason,
                viewModel.FreeBet));

            viewModel.Result = resultModel.Result;
            viewModel.Partners = resultModel.Partners;

            return View(viewModel);
        }
    }
}