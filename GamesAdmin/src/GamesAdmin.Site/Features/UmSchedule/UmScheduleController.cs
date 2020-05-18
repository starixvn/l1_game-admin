using System.Threading.Tasks;
using GamesAdmin.Site.Features.UmSchedule.ViewModels;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GamesAdmin.Site._Shared.Configurations;

namespace GamesAdmin.Site.Features.UmSchedule
{
    [Authorize]
    public class UmScheduleController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAppSettings appSettings;

        public UmScheduleController(IMediator mediator, IAppSettings appSettings)
        {
            this.mediator = mediator;
            this.appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public IActionResult Add()
        => View(new AddViewModel(appSettings));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateRequest request)
        {
            await mediator.Send(request);

            return RedirectToAction(nameof(Index));
        }       

        [HttpGet]
        public async Task<IActionResult> Edit()
        => View(await mediator.Send(new EditRequest()));
    }
}
