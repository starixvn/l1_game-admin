using MediatR;
using GamesAdmin.Site.Features.Currencies.ViewModels;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetExistingCurrencyGroupSuggestionsRequest : IRequest<CreateCurrencyWithSuggestionViewModel>
    {
        public string PartnerId { get; set; }
        public double Rate { get; set; }
        public bool GetFromAllOperators { get; set; }
    }
} 