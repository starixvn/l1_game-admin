using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CheckServiceStatusRequest : IRequest<StatusGameResult>
    {
        public CheckServiceStatusRequest(Player player, string gameName)
        {
            Player = player;
            GameName = gameName;
        }

        public Player Player { get; }

        public string GameName { get; }
    }
}
