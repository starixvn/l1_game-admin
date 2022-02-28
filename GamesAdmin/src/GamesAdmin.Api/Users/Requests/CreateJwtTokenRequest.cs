using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class CreateJwtTokenRequest : IRequest<string>
    {
        public CreateJwtTokenRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}