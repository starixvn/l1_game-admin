using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class UpdateRequest : IRequest<bool>
    {
        public UpdateRequest(AutoRNGGameConfig game)
        {
            Game = game;
        }

        public AutoRNGGameConfig Game { get; }
    }
}
