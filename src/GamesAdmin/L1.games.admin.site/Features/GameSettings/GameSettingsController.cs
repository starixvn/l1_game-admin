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
            return View();
        }
    }
}