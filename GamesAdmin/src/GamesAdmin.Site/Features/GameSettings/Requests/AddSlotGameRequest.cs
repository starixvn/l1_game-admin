using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class AddSlotGameRequest : IRequest<AddSlotGameViewModel>
    {
        public AddSlotGameRequest(GameType type)
        {
            Type = type;
        }

        public GameType Type { get; }
    }
}
