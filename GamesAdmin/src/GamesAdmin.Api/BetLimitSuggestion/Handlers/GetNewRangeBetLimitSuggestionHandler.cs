using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.BetLimitSuggestion.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.BetLimitSuggestion;
using GamesAdmin.Database.Entities;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.BetLimitSuggestion.Handlers
{
  public class GetNewRangeBetLimitSuggestionHandler : IRequestHandler<GetNewRangeBetLimitSuggestionRequest, NewRangeSuggestion>
  {
    private readonly ISentryClient sentryClient;
    private readonly IBetLimitSuggestionService betLimitSuggestionService;

    public GetNewRangeBetLimitSuggestionHandler(
      ISentryClient sentryClient,
      IBetLimitSuggestionService betLimitSuggestionService)
    {
      this.sentryClient = sentryClient;
      this.betLimitSuggestionService = betLimitSuggestionService;
    }

    public async Task<NewRangeSuggestion> Handle(GetNewRangeBetLimitSuggestionRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var baseCurrencyGroupGames = await betLimitSuggestionService.GetBaseCurrencyGroupGames(request.CurrencyGroupProposal, false);
        var availableChipValues = betLimitSuggestionService.GetAvailableChipValues();
        var response = new NewRangeSuggestion();

        foreach (var baseGame in baseCurrencyGroupGames.Values)
        {
          var adjustedGame = betLimitSuggestionService.CreateAdjustedCurrencyGroupGame(baseGame, request.CurrencyGroupProposal.RateToBase, availableChipValues);

          CategorizeGame(adjustedGame, response);
        }

        return response;
      }
      catch (Exception ex)
      {
        sentryClient.CaptureException(ex);
        return new NewRangeSuggestion();
      }
    }

    private void CategorizeGame(CurrencyGroupGame game, NewRangeSuggestion response)
    {
      bool hasMaxBetPerChoice = game.MaxBetPerChoices != null && game.MaxBetPerChoices.Count > 0;
      bool hasCoinValue = game.CoinValues != null && game.CoinValues.Length > 0;
      bool hasBetLevel = game.BetLevels != null && game.BetLevels.Length > 0;

      if (hasMaxBetPerChoice)
      {
        response.MaxBetPerChoiceGames.Add(game);
      }
      else if (hasCoinValue)
      {
        response.CoinValueGames.Add(game);
      }
      else if (hasBetLevel)
      {
        response.BetLevelGames.Add(game);
      }
      else
      {
        response.NoSpecialConfigGames.Add(game);
      }
    }

    private CurrencyGroupGame SelectExampleGame(IEnumerable<CurrencyGroupGameEntity> baseGames, double rate, List<int> availableChipValues)
    {
      var firstGame = baseGames.FirstOrDefault();
      if (firstGame != null)
      {
        return betLimitSuggestionService.CreateAdjustedCurrencyGroupGame(firstGame, rate, availableChipValues);
      }
      return null;
    }
  }
}