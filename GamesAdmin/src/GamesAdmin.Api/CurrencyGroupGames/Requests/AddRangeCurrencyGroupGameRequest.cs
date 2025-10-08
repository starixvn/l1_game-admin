using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class AddRangeCurrencyGroupGameRequest : IRequest<bool>
    {
        public AddRangeCurrencyGroupGameRequest(IEnumerable<CurrencyGroupGame> currencyGroupGames)
        {
            CurrencyGroupGames = currencyGroupGames;
        }

        public IEnumerable<CurrencyGroupGame> CurrencyGroupGames { get; }
    }
}
