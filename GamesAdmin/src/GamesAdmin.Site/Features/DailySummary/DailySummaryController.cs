namespace GamesAdmin.Site.Features.DailySummary
{
    using System.Threading.Tasks;
    using GamesAdmin.Site.Features.DailySummary.Requests;
    using GamesAdmin.Site.Features.DailySummary.ViewModels;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class DailySummaryController : Controller
    {
        private readonly IMediator mediator;

        public DailySummaryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await mediator.Send(new DailySummaryPageRequest()));
        }

        [HttpPost]
        public async Task<IActionResult> Index(DailySummaryViewModel model)
        {
            return Json(await mediator.Send(new GetDailySummaryRequest(model)));
        }
    }
}
