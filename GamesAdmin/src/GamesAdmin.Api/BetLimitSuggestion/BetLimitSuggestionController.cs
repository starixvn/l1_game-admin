using System.Threading.Tasks;
using GamesAdmin.Api._Shared;
using GamesAdmin.Api.BetLimitSuggestion.Requests;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api.BetLimitSuggestion
{
  [Route("api/betLimitSuggestion")]
  public class BetLimitSuggestionController : BaseAuthorizeController
  {
    private readonly IMediator mediator;

    public BetLimitSuggestionController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpPost("getBetLimitSuggestion")]
    public async Task<IActionResult> GetBetLimitSuggestion([FromBody] CurrencyGroupProposal currencyGroupProposal)
    {
      var result = await mediator.Send(new GetBetLimitSuggestionRequest(currencyGroupProposal));
      return Ok(result);
    }

    [HttpPost("getNewRangeBetLimitSuggestion")]
    public async Task<IActionResult> GetNewRangeBetLimitSuggestion([FromBody] CurrencyGroupProposal currencyGroupProposal)
    {
      var result = await mediator.Send(new GetNewRangeBetLimitSuggestionRequest(currencyGroupProposal));
      return Ok(result);
    }
  }
}