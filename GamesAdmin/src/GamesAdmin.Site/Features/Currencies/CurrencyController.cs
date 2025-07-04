using System.Threading.Tasks;
using Audit.Mvc;
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
            var codes = await mediator.Send(new GetExchangeCodesForSelectionRequest());
            var viewModel = new CreateCurrencyViewModel
            {
                Partners = partners,
                ExchangeCodes = codes
            };

            return PartialView("_AddNewCurrency", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateWithSuggestion()
        {
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());
            var codes = await mediator.Send(new GetExchangeCodesForSelectionRequest());
            var viewModel = new CreateCurrencyWithSuggestionViewModel
            {
                Partners = partners,
                ExchangeCodes = codes
            };

            return PartialView("_AddNewCurrencyWithSuggestion", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind] CreateCurrencyViewModel model)
        {
            var result = await mediator.Send(new CreateRequest(model));

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetExistingCurrencyGroupSuggestions([Bind] GetExistingCurrencyGroupSuggestionsRequest request)
        {
            var viewModel = await mediator.Send(request);
            return PartialView("_ExistingCurrencyGroupSuggestions", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrencyGroupGameSuggestions([Bind] GetCurrencyGroupGameSuggestionsRequest request)
        {
            var viewModel = await mediator.Send(request);
            return PartialView("_CurrencyGroupGameSuggestions", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateWithSuggestion([Bind] CreateCurrencyWithSuggestionViewModel model)
        {
            var result = await mediator.Send(new CreateWithSuggestionRequest(model));
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var viewModel = await mediator.Send(new GetUpdateCurrencyViewModelRequest { Id = id });

            return PartialView("_UpdateCurrency", viewModel);
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([Bind] UpdateCurrencyViewModel model)
        {
            var result = await mediator.Send(new UpdateRequest(model));

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrencies([Bind] GetCurrencyByPartnersModel model)
        {
            var viewModel = await mediator.Send(new GetByPartnersRequest(model));

            return PartialView("_Currencies", viewModel);
        }
    }
}