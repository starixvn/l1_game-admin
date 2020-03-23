using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class UpdateRequest: IRequest<bool>
    {
        public UpdateRequest(GameConfig game)
        {
            Game = game;
        }

        public GameConfig Game { get; }
    }
}
