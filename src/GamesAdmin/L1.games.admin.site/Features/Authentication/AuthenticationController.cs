using System.Collections.Generic;
using System.Security.Claims;
using L1.games.admin.core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace L1.games.admin.site.Features.Authentication
{
    public class AuthenticationController : Controller
    {
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind] User user)
        {
            var username = "admin";
            var password = "default";

            if (user.UserName.Equals(username) && user.Password.Equals(password))
            {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, $"{user.UserName}@luminetsoft.com"),
                 };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Account", "GameSettings");
            }
            return View();
        }
    }
}
