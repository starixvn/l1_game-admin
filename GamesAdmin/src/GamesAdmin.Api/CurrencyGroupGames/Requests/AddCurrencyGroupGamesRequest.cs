using System.Collections.Generic;
using GamesAdmin.Core.Models;
using MediatR;

namespace GamesAdmin.Api.CurrencyGroupGames.Requests
{
    public class AddCurrencyGroupGamesRequest : IRequest<bool>
    {
        public AddCurrencyGroupGamesRequest(IEnumerable<CurrencyGroupGame> currencyGroupGames)
        {
            CurrencyGroupGames = currencyGroupGames;
        }

        public IEnumerable<CurrencyGroupGame> CurrencyGroupGames { get; }
    }
} 