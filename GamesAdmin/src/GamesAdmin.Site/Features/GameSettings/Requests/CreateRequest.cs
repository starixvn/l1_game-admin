using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CreateRequest : IRequest<bool>
    {
        public CreateRequest(AutoRNGGameConfig game)
        {
            Game = game;
        }

        public AutoRNGGameConfig Game { get; }
    }
}
