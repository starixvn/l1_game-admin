using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class SignInRequest: IRequest<User>
    {
        public SignInRequest(string username, string password) 
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}
