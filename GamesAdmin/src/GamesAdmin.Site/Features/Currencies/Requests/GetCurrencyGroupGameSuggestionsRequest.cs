using MediatR;
using GamesAdmin.Site.Features.Currencies.ViewModels;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetCurrencyGroupGameSuggestionsRequest : IRequest<CreateCurrencyWithSuggestionViewModel>
    {
        public string CurrencyGroupId { get; set; }
        public double Rate { get; set; }
    }
} 