namespace GamesAdmin.Core.Models
{
    public class Player
    {
        public Player(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}
