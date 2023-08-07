using GamesAdmin.Core.Models.Lobby;
using MediatR;

namespace GamesAdmin.Api.RoundWatcher
{
    public class RoundWatcherRequest : IRequest<RoundWatcherResult>
    {
        public RoundWatcherRequest(Game[] games)
        {
            Games = games;
        }
        public Game[] Games { get; }
    }
}
