namespace GamesAdmin.Site.Features.Bot.Results
{
    public class AddBotResult
    {
        public AddBotResult(string username, string name, string password)
        {
            Username = username;
            Name = name;
            Password = password;
        }

        public string Username { get; }

        public string Name { get; }

        public string Password { get; }

        public string Error { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);
    }
}
