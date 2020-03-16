using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.Games.Requests
{
    public class UpdateRequest: IRequest<bool>
    {
        public UpdateRequest(Game game)
        {
            Game = game;
        }

        public Game Game { get; }
    }
}
