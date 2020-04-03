using GamesAdmin.Site._Shared.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features._Shared
{
    public class HomeController : Controller
    {
        private readonly IGameControlPanelSettings panelSettings;
        public HomeController(IGameControlPanelSettings panelSettings) 
        {
            this.panelSettings = panelSettings;
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RedirectToPanel()
        {
            return Redirect(panelSettings.Host);
        }
    }
}