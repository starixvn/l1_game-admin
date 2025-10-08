using System;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.BetLimitSuggestion.Requests;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.BetLimitSuggestion.Handlers
{
  public class GetBetLimitSuggestionHandler : IRequestHandler<GetBetLimitSuggestionRequest, BetLimitSuggestionResult>
  {
    private readonly ISentryClient sentryClient;
    private readonly IBetLimitSuggestionService betLimitSuggestionService;

    public GetBetLimitSuggestionHandler(
      ISentryClient sentryClient,
      IBetLimitSuggestionService betLimitSuggestionService)
    {
      this.sentryClient = sentryClient;
      this.betLimitSuggestionService = betLimitSuggestionService;
    }

    public async Task<BetLimitSuggestionResult> Handle(GetBetLimitSuggestionRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var baseCurrencyGroupGames = await betLimitSuggestionService.GetBaseCurrencyGroupGames(request.CurrencyGroupProposal);
        var candidateGroups = await betLimitSuggestionService.GetCandidateCurrencyGroups(request.CurrencyGroupProposal);

        var suggestions = await betLimitSuggestionService.ProcessCandidateGroups(candidateGroups, baseCurrencyGroupGames, request.CurrencyGroupProposal);

        return await betLimitSuggestionService.CreateSuggestionsResultAsync(suggestions);
      }
      catch (Exception ex)
      {
        sentryClient.CaptureException(ex);
        return new BetLimitSuggestionResult();
      }
    }
  }
}