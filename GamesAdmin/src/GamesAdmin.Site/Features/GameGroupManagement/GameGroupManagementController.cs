using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Audit.Mvc;
using GamesAdmin.Site.Features.GameGroupManagement.Requests;
using GamesAdmin.Site.Features.GameGroupManagement.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.GameGroupManagement
{
    public class GameGroupManagementController : Controller
    {
        private readonly IMediator mediator;

        public GameGroupManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await mediator.Send(new IndexPageRequet()));

        [Authorize(Roles = "admin"), HttpGet]
        public async Task<IActionResult> Create() => PartialView("_Create", await mediator.Send(new CreatePageRequest()));

        [Authorize(Roles = "admin"), HttpPost, Audit]
        public async Task<IActionResult> Create([Bind] CreateGameGroupViewModel model) => Ok(await mediator.Send(new CreateRequest(model)));

        [Authorize(Roles = "admin"), HttpGet]
        public async Task<IActionResult> Update([FromQuery] string groupId) => PartialView("_Update", await mediator.Send(new UpdatePageRequest(groupId)));

        [Authorize(Roles = "admin"), HttpPost, Audit]
        public async Task<IActionResult> Update([Bind] UpdateGameGroupViewModel model) => Ok(await mediator.Send(new UpdateRequest(model)));
    }
}