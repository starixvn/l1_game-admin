using System.Threading.Tasks;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.UmSchedule
{
    [Authorize]
    public class UmScheduleController : Controller
    {
        private readonly IMediator mediator;

        public UmScheduleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));

        [HttpGet]
        public async Task<IActionResult> Edit()
        => View(await mediator.Send(new EditRequest()));
    }
}
