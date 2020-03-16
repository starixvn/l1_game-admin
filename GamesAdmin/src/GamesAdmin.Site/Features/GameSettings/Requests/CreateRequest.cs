using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(Game game)
        {
            Game = game;
        }

        public Game Game { get; }
    }
}
