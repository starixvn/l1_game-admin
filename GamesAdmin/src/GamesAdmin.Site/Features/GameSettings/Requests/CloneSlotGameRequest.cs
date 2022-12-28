using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class CloneSlotGameRequest : IRequest<bool>
    {
        public CloneSlotGameRequest(string displayName, byte gameId, byte cloneFrom)
        {
            DisplayName = displayName;
            GameId = gameId;
            ClonedFrom = cloneFrom;
        }

        public string DisplayName { get; set; }

        public byte GameId { get; set; }

        public byte ClonedFrom { get; set; }
    }
}