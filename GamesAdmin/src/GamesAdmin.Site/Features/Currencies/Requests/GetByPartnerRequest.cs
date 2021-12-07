using GamesAdmin.Site.Features.Currencies.Models;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Requests
{
    public class GetByPartnerRequest : IRequest<CurrencyManagementViewModel>
    {
        public GetByPartnerRequest(GetCurrencyByPartnerModel model)
        {
            Model = model;
        }

        public GetCurrencyByPartnerModel Model { get; }
    }
}