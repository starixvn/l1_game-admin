namespace GamesAdmin.Site.Features.BetDetails
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.BetDetails.Requests;
    using GamesAdmin.Site.Features.BetDetails.ViewModels;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BetDetailsController : Controller
    {
        private readonly IMediator mediator;

        public BetDetailsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await mediator.Send(new BetDetailsPageRequest()));
        }

        [HttpPost]
        public async Task<IActionResult> Index(BetDetailsViewModel model)
        {
            var url = await mediator.Send(new GetUrlAccessBetDetailsRequest(model));

            return Redirect(url);
        }
    }
}
