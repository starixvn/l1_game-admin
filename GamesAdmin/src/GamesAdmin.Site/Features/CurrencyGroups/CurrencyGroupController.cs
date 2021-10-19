using System.Threading.Tasks;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.CurrencyGroups
{
    [Authorize]
    public class CurrencyGroupController : Controller
    {
        private readonly IMediator mediator;

        public CurrencyGroupController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await mediator.Send(new IndexPageRequest()));

        [HttpPost]
        public async Task<IActionResult> GetCurrencyGroups([Bind] QueryCurrencyGroupsModel model)
        {
            var viewModel = await mediator.Send(new QueryCurrencyGroupsRequest(model));

            return PartialView("_Groups", viewModel);
        }
    }
}