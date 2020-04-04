using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
