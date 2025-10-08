using GamesAdmin.Core.Models.BetLimitSuggestion;
using MediatR;

namespace GamesAdmin.Api.BetLimitSuggestion.Requests
{
  public class GetBetLimitSuggestionRequest : IRequest<BetLimitSuggestionResult>
  {

    public GetBetLimitSuggestionRequest(CurrencyGroupProposal currencyGroupProposal)
    {
      CurrencyGroupProposal = currencyGroupProposal;
    }

    public CurrencyGroupProposal CurrencyGroupProposal { get; set; }
  }
}