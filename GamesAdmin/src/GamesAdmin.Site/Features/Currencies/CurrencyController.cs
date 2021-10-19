using System.Threading.Tasks;
using GamesAdmin.Site.Features.Currencies.Models;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Currencies
{
    [Authorize]
    public class CurrencyController : Controller
    {
        private readonly IMediator mediator;

        public CurrencyController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View(await mediator.Send(new IndexRequest()));

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());
            var viewModel = new CreateCurrencyViewModel {
                Partners = partners
            };

            return PartialView("_AddNewCurrency", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind] CreateCurrencyViewModel model)
        {
            var result = await mediator.Send(new CreateRequest(model));

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var viewModel = await mediator.Send(new GetUpdateCurrencyViewModelRequest { Id = id });

            return PartialView("_UpdateCurrency", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update([Bind] UpdateCurrencyViewModel model)
        {
            var result = await mediator.Send(new UpdateRequest(model));

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrencies([Bind] GetCurrencyByPartnerModel model)
        {
            var viewModel = string.IsNullOrEmpty(model.PartnerId)
                ? await mediator.Send(new GetAllRequest())
                : await mediator.Send(new GetByPartnerRequest(model));

            return PartialView("_Currencies", viewModel);
        }
    }
}