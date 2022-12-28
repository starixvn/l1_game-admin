using MediatR;

namespace GamesAdmin.Api.GameSettings.Requests
{
    public class CloneNewSlotGameRequest : IRequest<bool>
    {
        public CloneNewSlotGameRequest(byte gameId, byte fromGameId, string displayName)
        {
            GameId = gameId;
            FromGameId = fromGameId;
            DisplayName = displayName;
        }

        public byte GameId { get; }

        public byte FromGameId { get; }

        public string DisplayName { get; }
    }
}