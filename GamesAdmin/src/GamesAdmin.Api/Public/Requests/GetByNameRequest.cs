using GamesAdmin.Api.Public.Models;
using MediatR;

namespace GamesAdmin.Api.Public.Requests
{
    public class GetGameConfigRequest : IRequest<GameConfig>
    {
        public GetGameConfigRequest(int gameId)
        {
            GameId = gameId;
        }

        public int GameId { get; }
    }
}