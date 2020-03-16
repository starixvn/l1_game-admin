using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamesAdmin.Site.Features.Authentication
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind] User user)
        {
            if (await authenticationService.SignInAsync(user, HttpContext))
            {
                return RedirectToAction("Index", "GameConfigs");
            }

            return View();
        }
    }
}
