using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.StatusChecker
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
