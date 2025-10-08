using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Site.Features.BetLimitSuggestion.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.BetLimitSuggestion.Requests
{
  public class GetNewRangeBetLimitSuggestionRequest : IRequest<NewRangeSuggestion>
  {
    public GetNewRangeBetLimitSuggestionRequest(GetBetLimitSuggestionModel model)
    {
      Model = model;
      CurrencyGroupProposal = new CurrencyGroupProposal
      {
        RateToBase = model.CurrencyRate,
        CandidateCurrencyGroupIds = model.CandidateCurrencyGroupIds,
        BaseCurrencyGroupId = model.BaseCurrencyGroupId
      };
    }

    public GetBetLimitSuggestionModel Model { get; set; }
    public CurrencyGroupProposal CurrencyGroupProposal { get; set; }
  }
}
