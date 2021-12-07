using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class CreateRequest : IRequest<MessageResult>
    {
        public CreateRequest(CreateCurrencyViewModel model)
        {
            Model = model;
        }

        public CreateCurrencyViewModel Model { get; }
    }
}