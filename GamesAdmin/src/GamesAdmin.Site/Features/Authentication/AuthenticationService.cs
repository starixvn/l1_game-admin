using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GamesAdmin.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace GamesAdmin.Site.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAsync(User user, HttpContext context);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private const string DefaultUser = "admin";
        private const string DefaultPassword = "default";

        public async Task<bool> SignInAsync(User user, HttpContext context)
        {
            if (user.UserName.Equals(DefaultUser) && user.Password.Equals(DefaultPassword))
            {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, $"{user.UserName}@luminetsoft.com"),
                 };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                await context.SignInAsync(userPrincipal);

                return true;
            }

            return false;
        }
    }
}
