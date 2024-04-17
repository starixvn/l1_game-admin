using GamesAdmin.Site._Shared.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features._Shared
{
    public class HomeController : Controller
    {
        private readonly IAppSettings appSettings;

        public HomeController(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Health() => new ContentResult
        {
            Content = $"Game Admin is running - {appSettings.Env} - {appSettings.SystemType}",
            ContentType = "text/html"
        };
    }
}