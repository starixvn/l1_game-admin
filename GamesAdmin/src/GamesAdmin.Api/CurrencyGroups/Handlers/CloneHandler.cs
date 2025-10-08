using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroupGames;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using MongoDB.Driver;
using Sentry;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
  public class CloneHandler : Handler, IRequestHandler<CloneRequest, CurrencyGroupModel>
  {
    private readonly ICurrencyGroupGameService _currencyGroupGameService;

    public CloneHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry, ICurrencyGroupGameService currencyGroupGameService)
      : base(mapper, repository, currency, sentry)
    {
      _currencyGroupGameService = currencyGroupGameService;
    }

    public async Task<CurrencyGroupModel> Handle(CloneRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var entity = Mapper.Map<CurrencyGroupEntity>(request.Model);
        await Repository.InsertOneAsync(entity);

        var newGroupId = entity.Id.ToString();

        var sourceGroupGames = await _currencyGroupGameService.GetByGroup(request.Model.SourceGroupId);

        var model = new CurrencyGroupModel(
          newGroupId,
          request.Model.Name,
          request.Model.Enabled,
          request.Model.PartnerId,
          request.Model.FractionalOddsFormat,
          new List<CurrencyModel>()
        );

        if (sourceGroupGames == null)
        {
          return model;
        }

        var sourceGroupGamesList = sourceGroupGames.ToList();

        if (sourceGroupGamesList.Any())
        {
          var clonedGames = sourceGroupGamesList.Select(game => new CurrencyGroupGame(
            newGroupId,
            game.GameId,
            game.GameName,
            game.Enabled,
            game.MinBet,
            game.MaxBet,
            game.MaxBetPerChoices,
            game.Chips,
            game.BetLevels,
            game.StakeLevels,
            game.CoinValues,
            game.Multiplier,
            game.MaxWinPerRound,
            game.DefaultCoinValue,
            game.DefaultBetLevel,
            game.OtherGameIdsInGroup
          )).ToList();

          var gamesAdded = await _currencyGroupGameService.AddRange(clonedGames);

          if (!gamesAdded)
          {
            await Repository.DeleteOneAsync(x => x.Id == entity.Id);
            return null;
          }
        }

        return model;
      }
      catch (System.Exception ex)
      {
        Sentry.CaptureException(ex);
        return null;
      }
    }
  }
}