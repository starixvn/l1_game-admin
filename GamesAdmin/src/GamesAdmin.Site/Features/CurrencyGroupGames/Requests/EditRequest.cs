using GamesAdmin.Core.Models;
using GamesAdmin.Site.Features._Shared;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class EditRequest : IRequest<ResultBase>
    {
        public EditRequest(CurrencyGroupGame currencyGroupGame)
        {
            CurrencyGroupGame = currencyGroupGame;
        }

        public CurrencyGroupGame CurrencyGroupGame { get; }
    }
}