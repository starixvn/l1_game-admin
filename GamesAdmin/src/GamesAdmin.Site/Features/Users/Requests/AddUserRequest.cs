using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.Users.Requests
{
    public class AddUserRequest : IRequest<ResultBase>
    {
        public AddUserRequest(User user) 
        {
            User = user;
        }

        public User User { get; }
    }
}
