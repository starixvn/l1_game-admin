using System.Threading.Tasks;
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
        public IActionResult Index() => View(new IndexViewModel());

        [HttpPost]
        public async Task<IActionResult> VoidTicket([Bind] IndexViewModel viewModel)
        {
            viewModel.Result = await mediator.Send(new VoidTicketRequest(
                viewModel.MemberId,
                viewModel.SiteId,
                viewModel.GameRoundId,
                (byte)viewModel.GameId,
                viewModel.Reason,
                viewModel.FreeBet));

            return View(viewModel);
        }
    }
}