namespace GamesAdmin.Core.Models
{
    public class User
    {
        public User(string username, string password, bool isAdmin) 
        {
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }

        public string Username { get; }

        public string Password { get; }

        public bool IsAdmin { get; }
    }
}
