using GamesAdmin.Core.Models;
using GamesAdmin.Core.Models.GameConfigs;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class CreateRequest: IRequest<GameCreationResult>
    {
        public CreateRequest(GameConfig game)
        {
            Game = game;
        }

        public GameConfig Game { get; }
    }
}
