using GamesAdmin.Site.Features._Shared;
using GamesAdmin.Site.Features.Promotions.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Promotions.Requests
{
    public class UpdateOperatorSettingRequest : IRequest<MessageResult>
    {
        public UpdateOperatorSettingRequest(EditCurrencyGroupSettingViewModel model)
        {
            Model = model;
        }

        public EditCurrencyGroupSettingViewModel Model { get; }
    }
}
