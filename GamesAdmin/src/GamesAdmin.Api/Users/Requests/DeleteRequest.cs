using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class DeleteRequest : IRequest<bool>
    {
        public DeleteRequest(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
