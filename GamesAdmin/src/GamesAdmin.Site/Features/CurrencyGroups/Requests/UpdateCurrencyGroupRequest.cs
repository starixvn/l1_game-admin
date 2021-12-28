using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Requests
{
    public class UpdateCurrencyGroupRequest : IRequest<MessageResult>
    {
        public UpdateCurrencyGroupRequest(UpdateCurrencyGroupViewModel model)
        {
            Model = model;
        }

        public UpdateCurrencyGroupViewModel Model { get; }
    }
}