using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class GetOneRequest : IRequest<EditViewModel>
    {
        public GetOneRequest(string currencyGroupId, int gameId)
        {
            CurrencyGroupId = currencyGroupId;
            GameId = gameId;
        }

        public string CurrencyGroupId { get;  }

        public int GameId { get; }
    }
}
