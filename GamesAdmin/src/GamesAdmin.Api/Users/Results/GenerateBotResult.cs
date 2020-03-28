namespace GamesAdmin.Api.Users.Results
{
    public class GenerateBotResult
    {
        public GenerateBotResult(string username, string name, int avatarId) 
        {
            Username = username;
            Name = name;
            AvatarId = avatarId;
        }

        public string Username { get; }

        public string Name { get; }

        public int AvatarId { get; }

        public string Error { get; set; }

        public bool IsSuccess => string.IsNullOrWhiteSpace(Error);
    }
}
