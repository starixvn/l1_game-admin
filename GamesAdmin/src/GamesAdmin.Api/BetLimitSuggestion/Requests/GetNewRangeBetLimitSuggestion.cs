using System.Collections.Generic;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using MediatR;

namespace GamesAdmin.Api.BetLimitSuggestion.Requests
{
  public class GetNewRangeBetLimitSuggestionRequest : IRequest<NewRangeSuggestion>
  {

    public GetNewRangeBetLimitSuggestionRequest(CurrencyGroupProposal currencyGroupProposal)
    {
      CurrencyGroupProposal = currencyGroupProposal;
    }

    public CurrencyGroupProposal CurrencyGroupProposal { get; set; }
  }
}