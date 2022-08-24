using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetGamesIdByTypeRequest : IRequest<IEnumerable<Game>>
    {
        public GameType GameType { get; set; }
        public GetGamesIdByTypeRequest(GameType gameType)
        {
            this.GameType = gameType;
        }
    }
}
