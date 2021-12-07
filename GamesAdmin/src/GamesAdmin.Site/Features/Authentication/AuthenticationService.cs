using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Sentry;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
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
        private const byte ExpiredHours = 3;
        private readonly IUserService userService;
        private readonly ISentryClient sentryClient;

        public AuthenticationService(IUserService userService, ISentryClient sentryClient)
        {
            this.userService = userService;
            this.sentryClient = sentryClient;
        }

        public async Task<bool> SignInAsync(Core.Models.Account user, HttpContext context)
        {
            try
            {
                var userSignin = await userService.SignIn(user.UserName.ToLowerInvariant(), user.Password);

                if (userSignin == null)
                {
                    sentryClient.CaptureMessage($"{DateTime.Now} Not found user {user.UserName}.");
                    return false;
                }

                var userRole = userSignin.IsAdmin ? Role.Admin.Value : Role.Operation.Value;
                var userClaims = new List<Claim>
                {
                    new Claim("Username", userSignin.Username),
                    new Claim(ClaimTypes.Email, $"{userSignin.Username}@saba.club"),
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim(ClaimTypes.Name, userSignin.Username)
                 };

                var grandmaIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                await context.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(grandmaIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(ExpiredHours)
                    });

                sentryClient.CaptureMessage($"{DateTime.Now} User {user.UserName} logged.");

                return true;
            }
            catch (Exception e)
            {
                sentryClient.CaptureException(e);
            }

            return false;
        }

        public Task SignOutAsync(HttpContext context)
        => context.SignOutAsync();
    }
}
