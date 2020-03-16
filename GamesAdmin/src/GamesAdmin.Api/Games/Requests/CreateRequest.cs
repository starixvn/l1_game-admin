using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class CreateRequest: IRequest<bool>
    {
        public CreateRequest(Game game)
        {
            Game = game;
        }

        public Game Game { get; }
    }
}
