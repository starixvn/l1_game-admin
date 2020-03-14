using System.Collections.Generic;
using L1.games.admin.core.Models;
using L1.games.admin.site.Features.GameSettings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L1.games.admin.site.Features.GameSettings
{
    [Authorize]
    public class GameSettingsController : Controller
    {
        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Games()
        {
            var gameViewModel = new GamesViewModel(new List<Game> 
            {
                new Game(1, "Cannon", true),
                new Game(2, "Clan Castle", false)
            }); 

            return View(gameViewModel);
        }
    }
}