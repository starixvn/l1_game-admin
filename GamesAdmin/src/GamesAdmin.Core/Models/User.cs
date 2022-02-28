namespace GamesAdmin.Core.Models
{
    public class User
    {
        public User(string username, string password, bool isAdmin, string jwtToken = "")
        {
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
            JwtToken = jwtToken;
        }

        public string Username { get; }

        public string Password { get; }

        public bool IsAdmin { get; }

        public string JwtToken { get; }
    }
}
