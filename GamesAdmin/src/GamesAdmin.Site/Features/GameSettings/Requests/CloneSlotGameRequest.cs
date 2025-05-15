using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CloneSlotGameRequest : IRequest<string>
    {
        public CloneSlotGameRequest(string displayName, int gameId, int cloneFrom, byte gameType)
        {
            DisplayName = displayName;
            GameId = gameId;
            ClonedFrom = cloneFrom;
            GameType = gameType;
        }

        public string DisplayName { get; }

        public int GameId { get; }

        public int ClonedFrom { get; }

        public byte GameType { get; }
    }
}