using GamesAdmin.Core.Enumeration;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameServers.Requests
{
    public class OnlineUserRequest: IRequest<OnlineUsers>
    {
        public OnlineUserRequest(string game = null) 
        {
            Game = game;
        }

        public string Game { get; }
    }
}
