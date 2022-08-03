namespace GamesAdmin.Site.Features.RetrieveEndRound
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using GamesAdmin.Site.Features.RetrieveEndRound.Request;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RetrieveEndRoundController : Controller
    {
        private readonly IMediator mediator;

        public RetrieveEndRoundController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind] BetDetailsViewModel model)
            => Json(await mediator.Send(new GetRetrieveEndRoundRequest(model)));
    }
}
