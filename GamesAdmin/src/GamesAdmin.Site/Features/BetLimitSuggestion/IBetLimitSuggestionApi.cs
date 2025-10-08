using System.Threading.Tasks;
using System.Collections.Generic;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace GamesAdmin.Site.Features.BetLimitSuggestion
{
  public interface IBetLimitSuggestionApi : IBaseAuthorizationApi
  {
    [Post("/betLimitSuggestion/getBetLimitSuggestion")]
    Task<BetLimitSuggestionResult> GetBetLimitSuggestion([FromBody] CurrencyGroupProposal currencyGroupProposal);

    [Post("/betLimitSuggestion/getNewRangeBetLimitSuggestion")]
    Task<NewRangeSuggestion> GetNewRangeBetLimitSuggestion([FromBody] CurrencyGroupProposal currencyGroupProposal);
  }
}