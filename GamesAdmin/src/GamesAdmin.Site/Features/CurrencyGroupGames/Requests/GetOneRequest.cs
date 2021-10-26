using GamesAdmin.Site.Features.CurrencyGroupGames.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroupGames.Requests
{
    public class GetOneRequest : IRequest<EditViewModel>
    {
        public string CurrencyGroupId { get; set; }

        public string GameId { get; set; }
    }
}
