using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class UpdateRequest : IRequest<MessageResult>
    {
        public UpdateRequest(UpdateCurrencyViewModel model)
        {
            Model = model;
        }

        public UpdateCurrencyViewModel Model { get; }
    }
}