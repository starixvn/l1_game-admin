using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.ChangePassword
{
    public class ChangePasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}