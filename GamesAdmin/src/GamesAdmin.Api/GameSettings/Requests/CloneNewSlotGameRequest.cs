using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class CloneNewSlotGameRequest : IRequest<string>
    {
        public CloneNewSlotGameRequest(int gameId, int fromGameId, string displayName)
        {
            GameId = gameId;
            FromGameId = fromGameId;
            DisplayName = displayName;
        }

        public int GameId { get; }

        public int FromGameId { get; }

        public string DisplayName { get; }
    }
}