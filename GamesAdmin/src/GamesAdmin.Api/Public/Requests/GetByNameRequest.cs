using GamesAdmin.Api.Public.Models;
using MediatR;

namespace GamesAdmin.Api.Public.Requests
{
    public class GetGameConfigRequest : IRequest<GameConfig>
    {
        public GetGameConfigRequest(byte gameId)
        {
            GameId = gameId;
        }

        public byte GameId { get; }
    }
}