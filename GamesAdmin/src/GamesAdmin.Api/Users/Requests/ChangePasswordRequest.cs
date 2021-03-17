using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class ChangePasswordRequest: IRequest<bool>
    {
        public ChangePasswordRequest(string username, string password, string newPassword) 
        {
            Username = username;
            Password = password;
            NewPassword = newPassword;
        }

        public string Username { get; }

        public string Password { get; }

        public string NewPassword { get; }
    }
}
