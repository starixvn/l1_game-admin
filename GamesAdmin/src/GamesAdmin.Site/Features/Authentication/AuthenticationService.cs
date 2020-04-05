using GamesAdmin.Site._Shared.Configurations;
using GamesAdmin.Site.Features.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GamesAdmin.Site.Features.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> SignInAsync(Core.Models.Account user, HttpContext context);

        Task SignOutAsync(HttpContext context);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private const string DefaultUser = "admin";
        private readonly IAppSettings appSettings;
        //private readonly IUserService userService;

        public AuthenticationService(IAppSettings appSettings, IUserService userService) 
        {
            this.appSettings = appSettings;
            //this.userService = userService;
        }

        public async Task<bool> SignInAsync(Core.Models.Account user, HttpContext context)
        {
            if (user.UserName.Equals(DefaultUser) && BCrypt.Net.BCrypt.Verify(user.Password, appSettings.Hash))
            {
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, $"{user.UserName}@luminetsoft.com"),
                 };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new[] { grandmaIdentity }),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3)
                    });

                return true;
            }

            return false;
        }

        public Task SignOutAsync(HttpContext context)
        => context.SignOutAsync();
    }
}
