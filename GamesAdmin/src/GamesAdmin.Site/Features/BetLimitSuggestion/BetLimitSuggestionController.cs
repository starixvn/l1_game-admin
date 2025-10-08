using Audit.Mvc;
using GamesAdmin.Site.Features.BetLimitSuggestion.Requests;
using GamesAdmin.Site.Features.BetLimitSuggestion.ViewModels;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.BetLimitSuggestion
{
    public class BetLimitSuggestionController : Controller
    {
        private readonly IMediator mediator;
        public BetLimitSuggestionController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IActionResult> IndexAsync(CurrencyNeedToSuggestModel model)
        {
            var currencyGroupFilterConditionModel = await mediator.Send(new GetCurrencyGroupFilterConditionRequest());
            var betLimitSuggestionModel = new BetLimitSuggestionModel
            {
                CurrencyNeedToSuggest = model,
                CurrencyGroupFilterCondition = currencyGroupFilterConditionModel
            };
            return PartialView("_Index", betLimitSuggestionModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetBetLimitSuggestion([FromBody] GetBetLimitSuggestionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(new GetBetLimitSuggestionRequest(model));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetNewRangeBetLimitSuggestion([FromBody] GetBetLimitSuggestionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(new GetNewRangeBetLimitSuggestionRequest(model));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartnersAndExchangeCodes()
        {
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());
            var codes = await mediator.Send(new GetExchangeCodesForSelectionRequest());

            return Json(new { partners, codes });
        }

        [HttpPost, Audit]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddCurrrencyToGroup([Bind] CreateCurrencyGroupViewModel model)
        {
            var result = await mediator.Send(new CreateCurrencyGroupRequest(model));

            return Json(result);
        }
    }
}

