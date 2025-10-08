using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Site.Features.BetLimitSuggestion.Requests;
using MediatR;

namespace GamesAdmin.Site.Features.BetLimitSuggestion.Handlers
{
  public class GetBetLimitSuggestionHandler : IRequestHandler<GetBetLimitSuggestionRequest, BetLimitSuggestionResult>
  {
    private readonly IBetLimitSuggestionApi betLimitSuggestionApi;

    public GetBetLimitSuggestionHandler(IBetLimitSuggestionApi betLimitSuggestionApi)
    {
      this.betLimitSuggestionApi = betLimitSuggestionApi;
    }

    public async Task<BetLimitSuggestionResult> Handle(GetBetLimitSuggestionRequest request, CancellationToken cancellationToken)
    {
      var proposal = new CurrencyGroupProposal
      {
        RateToBase = request.Model.CurrencyRate,
        CandidateCurrencyGroupIds = request.Model.CandidateCurrencyGroupIds.ToArray(),
        BaseCurrencyGroupId = request.Model.BaseCurrencyGroupId
      };

      return await betLimitSuggestionApi.GetBetLimitSuggestion(proposal);
    }
  }
}
