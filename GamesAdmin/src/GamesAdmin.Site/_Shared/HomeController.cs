using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site._Shared
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}