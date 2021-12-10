using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class GetCurrenctGroupGameRequest : IRequest<CurrencyGroupGame>
    {
        public GetCurrenctGroupGameRequest(string currencyGroupId, int gameId)
        {
            CurrencyGroupId = currencyGroupId;
            GameId = gameId;
        }

        public string CurrencyGroupId { get; }
        public int GameId { get; }
    }
}