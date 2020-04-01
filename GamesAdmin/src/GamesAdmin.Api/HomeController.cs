using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Api
{
    public class HomeController : Controller
    {
        public IActionResult Index()
            => new ContentResult
            {
                Content = "Games Admin API is working."
            };
    }
}
