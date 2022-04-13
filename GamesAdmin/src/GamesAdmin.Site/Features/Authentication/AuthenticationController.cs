using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var isLoginSuccessfully = await authenticationService.SignInAsync(user, HttpContext);
            if (isLoginSuccessfully)
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}