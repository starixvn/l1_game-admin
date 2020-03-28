using GamesAdmin.Api._Shared;

namespace GamesAdmin.Api.Users.Results
{
    public class GenerateUserResult : BaseResult
    {
        public GenerateUserResult(string username, string name, string password)
        {
            Username = username;
            Name = name;
            Password = password;
        }

        public string Username { get; }

        public string Name { get; }

        public string Password { get; }
    }
}
