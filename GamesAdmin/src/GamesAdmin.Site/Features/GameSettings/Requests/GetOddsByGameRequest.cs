using System.Collections.Generic;
using GamesAdmin.Core.Enumeration;
using GamesAdmin.Site.Features.GameSettings.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.GameSettings.Requests
{
    public class GetOddsByGameRequest : IRequest<IEnumerable<OddsViewModel>>
    {
        public GetOddsByGameRequest(GameId gameId)
        {
            this.GameName = Game.FromGameId(gameId).Value;
        }
        public string GameName { get; }
    }
}
