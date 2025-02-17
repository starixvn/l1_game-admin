using System;
using System.Linq;
using System.Threading.Tasks;
using GamesAdmin.Site._Shared.Services.GeneralGameSettings;
using GamesAdmin.Site.Features._Shared.Components.SelectGame;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesAdmin.Site._Shared.ViewComponents
{
    public class SelectGame : ViewComponent
    {
        private readonly IGeneralGameSettingService generalGameSettingService;

        public SelectGame(IGeneralGameSettingService generalGameSettingService)
        {
            this.generalGameSettingService = generalGameSettingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, bool selectAll = false)
        {
            try
            {
                var games = await generalGameSettingService.GetAllGames();

                var items = games.Select(game => new SelectListItem { Text = game.DisplayName, Value = game.GameId.ToString() });

                return View(new SelectGameModel(name, items, selectAll));
            }
            catch (Exception ex)
            {
                Sentry.SentrySdk.CaptureException(ex);
            }

            return View(new SelectGameModel(name, Enumerable.Empty<SelectListItem>(), selectAll));
        }
    }
}