using GamesAdmin.Database.Entities.Lobby;
using MediatR;

namespace GamesAdmin.Api.RoundWatcher
{
    public class RoundWatcherRequest : IRequest<bool>
    {
        public RoundWatcherRequest(string[] currencyGroupIds, RoundTimerGameEntity roundTimerGame)
        {
            this.currencyGroupIds = currencyGroupIds;
            this.roundTimerGame = roundTimerGame;
        }
        public string[] currencyGroupIds { get; }

        public RoundTimerGameEntity roundTimerGame { get; set; }
    }
}
