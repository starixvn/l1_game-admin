using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GamesAdmin.Api.GeneralGameSettings.Requests;
using GamesAdmin.Core.Models;
using MediatR;
using System.Linq;
using GamesAdmin.Api.CurrencyGroups;
using GamesAdmin.Api.CurrencyGroupGames;

namespace GamesAdmin.Api.GeneralGameSettings
{
    public class GeneralGameSettingHandler :
        IRequestHandler<GetAllRequest, IEnumerable<GeneralGameSetting>>,
        IRequestHandler<GetGameByPartnerAndTypeRequest, IEnumerable<GeneralGameSetting>>
    {
        private readonly IGeneralGameSettingService generalGameSettingService;
        private readonly ICurrencyGroupService currencyGroupService;
        private readonly ICurrencyGroupGameService currencyGroupGameService;

        public GeneralGameSettingHandler(IGeneralGameSettingService generalGameSettingService,
            ICurrencyGroupService currencyGroupService, 
            ICurrencyGroupGameService currencyGroupGameService)
        {
            this.generalGameSettingService = generalGameSettingService;
            this.currencyGroupService = currencyGroupService;
            this.currencyGroupGameService = currencyGroupGameService;
        }

        public Task<IEnumerable<GeneralGameSetting>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => generalGameSettingService.GetAll();

        public async Task<IEnumerable<GeneralGameSetting>> Handle(GetGameByPartnerAndTypeRequest request, CancellationToken cancellationToken)
        {
            var groups = await currencyGroupService.GetByPartner(request.PartnerId);
            var games = await currencyGroupGameService.GetAll();
            var gameSettings = await generalGameSettingService.GetAll();
            var partnerGroupIds = groups.Select(g => g.Id).ToHashSet();

            var filteredGames = from game in games
                                join setting in gameSettings on game.GameId equals setting.GameId
                                where partnerGroupIds.Contains(game.GroupId)
                                      && (int)setting.Type == request.GameType
                                select setting;

            var distinctGames = filteredGames
                .GroupBy(g => g.GameId)
                .Select(g => g.First())
                .ToList();

            return distinctGames;
        }
    }
}