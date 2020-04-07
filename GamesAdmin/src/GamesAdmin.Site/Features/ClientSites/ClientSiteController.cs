using System.Threading.Tasks;
using GamesAdmin.Site.Features.ClientSites.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.ClientSites
{
    [Authorize(Roles = "admin")]
    public class ClientSiteController : Controller
    {
        private readonly IMediator mediator;

        public ClientSiteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var clientSites = await mediator.Send(new GetAllRequest());
            return View(clientSites);
        }
    }
}