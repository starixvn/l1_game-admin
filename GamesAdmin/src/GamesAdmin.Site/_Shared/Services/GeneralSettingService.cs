using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features.GameSettings;

namespace GamesAdmin.Site._Shared.Services
{
    public interface IGeneralGameSettingService
    {
        Task<IEnumerable<GeneralGameSetting>> GetAllGames();
    }

    public class GeneralGameSettingService : IGeneralGameSettingService
    {
        private readonly IGameSettingService gameSettingService;

        public GeneralGameSettingService(IGameSettingService gameSettingService)
        {
            this.gameSettingService = gameSettingService;
        }

        public async Task<IEnumerable<GeneralGameSetting>> GetAllGames()
        {
            var games = await gameSettingService.GetAll();

            var oldGameIds = Enumeration.GetAll<Game>()
                .Where(game => (byte)game.GameId != 0)
                .Select(game => new GeneralGameSetting((int)game.GameId, game.Value, game.DisplayName))
                .ToList();

            var newGameIds = games
                .Where(game => game.Info.GameId != 0 && !oldGameIds.Exists(x => x.GameId == game.Info.GameId))
                .Select(game => new GeneralGameSetting(game.Info.GameId, game.Info.Name, game.Info.DisplayName))
                .ToList();

            return oldGameIds.Concat(newGameIds);
        }
    }
}