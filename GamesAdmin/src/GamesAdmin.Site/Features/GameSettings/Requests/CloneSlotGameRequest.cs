using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CloneSlotGameRequest : IRequest<bool>
    {
        public CloneSlotGameRequest(string displayName, int gameId, byte cloneFrom)
        {
            DisplayName = displayName;
            GameId = gameId;
            ClonedFrom = cloneFrom;
        }

        public string DisplayName { get; }

        public int GameId { get; }

        public byte ClonedFrom { get; }
    }
}