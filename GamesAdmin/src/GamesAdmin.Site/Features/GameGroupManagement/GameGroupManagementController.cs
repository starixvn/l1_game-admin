using System.Threading.Tasks;
using Audit.Mvc;
using GamesAdmin.Site.Features.GameGroupManagement.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.GameGroupManagement
{
    [Authorize]
    public class GameGroupManagementController : Controller
    {
        private readonly IMediator mediator;

        public GameGroupManagementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View();
        [HttpGet]
        public async Task<IActionResult> Create() => PartialView("_Create", await mediator.Send(new CreatePageRequest()));
    }
}