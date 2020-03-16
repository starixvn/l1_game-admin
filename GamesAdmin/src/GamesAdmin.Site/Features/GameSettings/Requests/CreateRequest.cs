using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(GameConfig game)
        {
            Game = game;
        }

        public GameConfig Game { get; }
    }
}
