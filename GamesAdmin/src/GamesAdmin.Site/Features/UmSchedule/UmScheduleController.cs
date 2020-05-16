using System.Threading.Tasks;
using GamesAdmin.Site.Features.UmSchedule.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace GamesAdmin.Site.Features.UmSchedule
{
    [Authorize]
    public class UmScheduleController : Controller
    {
        private readonly IMediator mediator;
        private readonly ISentryClient sentryClient;

        public UmScheduleController(IMediator mediator, ISentryClient sentryClient)
        {
            this.mediator = mediator;
            this.sentryClient = sentryClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public async Task<IActionResult> Add()
        => View(await mediator.Send(new AddRequest()));
    }
}
