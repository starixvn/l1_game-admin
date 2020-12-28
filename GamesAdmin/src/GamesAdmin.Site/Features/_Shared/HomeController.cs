using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features._Shared
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}