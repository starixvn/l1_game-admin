using System.Threading.Tasks;
using GamesAdmin.Site.Features.ApiLogs.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.ApiLogs
{
    [Authorize]
    public class ApiLogController : Controller
    {
        private readonly IMediator mediator;

        public ApiLogController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await mediator.Send(new ApiLogPageRequest()));

        [HttpPost]
        public async Task<IActionResult> Query(QueryApiLogRequest request)
            => PartialView("_Logs", await mediator.Send(request));
    }
}