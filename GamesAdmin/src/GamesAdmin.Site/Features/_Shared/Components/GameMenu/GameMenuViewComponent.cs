using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared.Components.ViewModels;
using GamesAdmin.Site.Features.GameSettings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features._Shared.Components
{
    public class GameMenuViewComponent : ViewComponent
    {
        private readonly IGameService gameService;

        public GameMenuViewComponent(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();

            return View(new MenuViewModel(items.ToList()));
        }

        private Task<IList<GameConfig>> GetItemsAsync()
        => gameService.GetGames();
    }
}
