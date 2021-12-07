using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class CreateCurrencyGroupPageHandler : Handler, IRequestHandler<CreateCurrencyGroupPageRequest, CreateCurrencyGroupViewModel>
    {
        private readonly ICurrencyService currencyService;
        private readonly IPartnerService partnerService;

        public CreateCurrencyGroupPageHandler(IMapper mapper, ICurrencyGroupApi api, ICurrencyService currencyService, IPartnerService partnerService) : base(mapper, api)
        {
            this.currencyService = currencyService;
            this.partnerService = partnerService;
        }

        public async Task<CreateCurrencyGroupViewModel> Handle(CreateCurrencyGroupPageRequest request, CancellationToken cancellationToken)
        {
            var parterItems = await partnerService.GetPartnersForSelection(partner => true);
            var currencies = await currencyService.GetCurrenciesForSelection(model => string.IsNullOrEmpty(model.GroupId) && model.Enabled);

            return new CreateCurrencyGroupViewModel
            {
                CurrencyItems = currencies,
                Currencies = new List<string>(),
                PartnerItems = parterItems
            };
        }
    }
}