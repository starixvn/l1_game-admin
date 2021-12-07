using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
    public class CreateCurrencyGroupRequest : IRequest<MessageResult>
    {
        public CreateCurrencyGroupRequest(CreateCurrencyGroupViewModel model)
        {
            Model = model;
        }

        public CreateCurrencyGroupViewModel Model { get; }
    }
}