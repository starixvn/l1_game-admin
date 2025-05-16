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
        IRequestHandler<GetGameByCurrencyGroupsRequest, IEnumerable<GeneralGameSetting>>
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

        public async Task<IEnumerable<GeneralGameSetting>> Handle(GetGameByCurrencyGroupsRequest request, CancellationToken cancellationToken)
        {
            var selectedGroupIds = request.CurrencyGroupIds.ToHashSet();
            var totalGroupCount = selectedGroupIds.Count;
            var gameIds = request.GameIds?.ToHashSet();

            var games = await currencyGroupGameService.GetAll();
            var gameSettings = await generalGameSettingService.GetAll();

            var settingDict = gameSettings
                .Where(s => (int)s.Type == request.GameType)
                .ToDictionary(s => s.GameId, s => s);

            var filtered = games
                 .Where(g =>
                     selectedGroupIds.Contains(g.GroupId) &&
                     (g.Enabled || (gameIds != null && gameIds.Contains(g.GameId))) &&
                     settingDict.ContainsKey(g.GameId)
                 )
                 .Select(g => new { g.GameId, g.GroupId, Setting = settingDict[g.GameId] });

            var result = filtered
                .GroupBy(x => x.GameId)
                .Where(g => g.Select(x => x.GroupId).Distinct().Count() == totalGroupCount)
                .Select(g => g.First().Setting);

            return result;
        }
    }
}