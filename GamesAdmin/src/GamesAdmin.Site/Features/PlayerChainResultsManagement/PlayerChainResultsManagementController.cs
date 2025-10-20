using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.PlayerChainResultsManagement
{
    public class PlayerChainResultsManagementController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "PlayerChainResultsManagement - Hello";
            return View();
        }
    }
}
