using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class GetConfigRequest : IRequest<CurrencyGroupGameConfig>
    {
        public GetConfigRequest(string currencyGroupId, int gameId, string type)
        {
            CurrencyGroupId = currencyGroupId;
            GameId = gameId;
            Type = type;
        }
        public string CurrencyGroupId { get; }
        public int GameId { get; }
        public string Type { get; }
    }
}