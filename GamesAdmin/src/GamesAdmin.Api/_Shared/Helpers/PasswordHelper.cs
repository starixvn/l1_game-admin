using System;

namespace GamesAdmin.Api._Shared.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

        public static string Random()
            => $"pwd{(new Random()).Next(200, 1000)}{new Random().Next(150, 500)}";

        public static bool VerifyHashPassword(string password, string hash)
            => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
