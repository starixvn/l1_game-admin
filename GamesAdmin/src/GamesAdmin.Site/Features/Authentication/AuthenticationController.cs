using System.Threading.Tasks;
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
        public async Task<IActionResult> Login([Bind] Core.Models.Account user)
        {
            if (await authenticationService.SignInAsync(user, HttpContext))
            {
                return RedirectToAction("Index", "GameSettings");
            }

            return RedirectToAction(nameof(LoginError));
        }

        public IActionResult LoginError()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await authenticationService.SignOutAsync(HttpContext);

            return RedirectToAction(nameof(UserLogin));
        }
    }
}