using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Forecast
{
    [Authorize]
    public class ForecastController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}