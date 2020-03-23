using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class CreateRequest: IRequest<bool>
    {
        public CreateRequest(GameConfig game)
        {
            Game = game;
        }

        public GameConfig Game { get; }
    }
}
