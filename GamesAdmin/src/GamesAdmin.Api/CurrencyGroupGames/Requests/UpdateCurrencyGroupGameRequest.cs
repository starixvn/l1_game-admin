using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class UpdateCurrencyGroupGameRequest : IRequest<bool>
    {
        public UpdateCurrencyGroupGameRequest(CurrencyGroupGame currencyGroupGame)
        {
            CurrencyGroupGame = currencyGroupGame;
        }

        public CurrencyGroupGame CurrencyGroupGame { get; }
    }
}