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
            if (authenticationService.SignInAsync(user, HttpContext).Result)
            {
                return RedirectToAction("Index", "Dashboard");
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