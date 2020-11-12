using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
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
