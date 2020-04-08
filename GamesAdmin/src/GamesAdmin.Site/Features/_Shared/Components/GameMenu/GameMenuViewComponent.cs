using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using GamesAdmin.Site._Shared.Enumerations;
using GamesAdmin.Site.Features._Shared.Components.ViewModels;
using GamesAdmin.Site.Features.GameSettings;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features._Shared.Components
{
    public class GameMenuViewComponent : ViewComponent
    {
        private readonly IGameSettingService gameService;
        private readonly ISentryClient sentryClient;

        public GameMenuViewComponent(IGameSettingService gameService, ISentryClient sentryClient)
        {
            this.gameService = gameService;
            this.sentryClient = sentryClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var gameTypes = new List<GameType>();
            var items = await GetItemsAsync();

            foreach (var item in items)
            {
                try {
                    var gameType = Enumeration.FromValue<GameType>(item.Name);
                    gameTypes.Add(gameType);
                } 
                catch (Exception ex) 
                {
                    sentryClient.CaptureException(ex);
                }
            }

            return View(new MenuViewModel(gameTypes));
        }

        private Task<IList<GameConfig>> GetItemsAsync()
        => gameService.GetGames();
    }
}
