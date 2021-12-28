using GamesAdmin.Site.Features.Currencies.Models;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetByPartnersRequest : IRequest<CurrencyManagementViewModel>
    {
        public GetByPartnersRequest(GetCurrencyByPartnersModel model)
        {
            Model = model;
        }

        public GetCurrencyByPartnersModel Model { get; }
    }
}