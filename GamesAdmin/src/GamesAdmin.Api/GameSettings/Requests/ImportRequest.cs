using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class ImportRequest : IRequest<bool>
    {
        public ImportRequest(GameConfig game)
        {
            Game = game;
        }

        public GameConfig Game { get; }
    }
}
