using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GamesAdmin.Site.Features._Shared
{
    public static class UserPermission
    {
        public static bool HasAdminPermission(this HttpContext context)
            => context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "admin";
    }
}