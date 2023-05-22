using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CloneSlotGameRequest : IRequest<string>
    {
        public CloneSlotGameRequest(string displayName, int gameId, int cloneFrom)
        {
            DisplayName = displayName;
            GameId = gameId;
            ClonedFrom = cloneFrom;
        }

        public string DisplayName { get; }

        public int GameId { get; }

        public int ClonedFrom { get; }
    }
}