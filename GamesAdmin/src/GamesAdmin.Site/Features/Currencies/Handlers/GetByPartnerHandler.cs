using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.Currencies.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class GetByPartnerHandler : Handler, IRequestHandler<GetByPartnerRequest, CurrencyManagementViewModel>
    {
        public GetByPartnerHandler(IMapper mapper, ICurrencyApi api) : base(mapper, api)
        {
        }

        public async Task<CurrencyManagementViewModel> Handle(GetByPartnerRequest request, CancellationToken cancellationToken)
        {
            var models = await Api.GetByPartner(request.Model);

            return new CurrencyManagementViewModel
            {
                Models = models
            };
        }
    }
}