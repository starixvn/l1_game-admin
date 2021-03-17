using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Users.Requests
{
    public class ChangePasswordRequest : IRequest<ResultBase>
    {
        public ChangePasswordRequest(string username, string password, string newPassword)
        {
            Username = username;
            Password = password;
            NewPassword = newPassword;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}
