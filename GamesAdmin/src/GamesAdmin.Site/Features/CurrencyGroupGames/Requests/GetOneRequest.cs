using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class GetOneRequest : IRequest<EditViewModel>
    {
        public GetOneRequest(string currencyGroupId, string gameName)
        {
            CurrencyGroupId = currencyGroupId;
            GameName = gameName;
        }

        public string CurrencyGroupId { get;  }

    
        public string GameName { get; }
    }
}
