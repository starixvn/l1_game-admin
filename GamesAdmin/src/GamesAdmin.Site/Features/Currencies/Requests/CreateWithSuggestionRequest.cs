using MediatR;
using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Currencies.ViewModels;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class CreateWithSuggestionRequest : IRequest<MessageResult>
    {
        public CreateCurrencyWithSuggestionViewModel Model { get; set; }

        public CreateWithSuggestionRequest(CreateCurrencyWithSuggestionViewModel model)
        {
            Model = model;
        }
    }
} 