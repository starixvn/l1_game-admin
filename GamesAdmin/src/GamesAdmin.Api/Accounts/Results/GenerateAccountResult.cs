using GamesAdmin.Api._Shared;

namespace GamesAdmin.Api.Accounts.Results
{
    public class GenerateAccountResult : BaseResult
    {
        public GenerateAccountResult(string username, string name, string password)
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
