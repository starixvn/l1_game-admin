using System.Threading.Tasks;
using GamesAdmin.Site.Features.CurrencyGroups.Models;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = await mediator.Send(new CreateCurrencyGroupPageRequest());

            return PartialView("_AddCurrencyGroup", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind] CreateCurrencyGroupViewModel model)
        {
            var result = await mediator.Send(new CreateCurrencyGroupRequest(model));

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var viewModel = await mediator.Send(new UpdateCurrencyGroupPageRequest(id));

            return PartialView("_UpdateCurrencyGroup", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update([Bind] UpdateCurrencyGroupViewModel model)
        {
            var result = await mediator.Send(new UpdateCurrencyGroupRequest(model));

            return Json(result);
        }
    }
}