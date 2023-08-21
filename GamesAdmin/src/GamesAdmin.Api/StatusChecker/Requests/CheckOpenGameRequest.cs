using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.StatusChecker
{
    public class CheckOpenGameRequest : IRequest<StatusGameResult>
    {
        public CheckOpenGameRequest(Player player, int[] games)
        {
            this.player = player;
            this.games = games;
        }

        public Player player { get; }

        public int[] games { get; }
    }
}
