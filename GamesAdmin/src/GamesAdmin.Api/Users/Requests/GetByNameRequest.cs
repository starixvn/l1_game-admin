using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Users.Requests
{
    public class GetByNameRequest: IRequest<User>
    {
        public GetByNameRequest(string username) 
        {
            Username = username;
        }

        public string Username { get; }
    }
}
