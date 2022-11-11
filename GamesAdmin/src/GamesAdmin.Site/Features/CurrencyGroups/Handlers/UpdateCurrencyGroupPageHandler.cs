using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using GamesAdmin.Site.Features.Partners;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class UpdateCurrencyGroupPageHandler : Handler, IRequestHandler<UpdateCurrencyGroupPageRequest, UpdateCurrencyGroupViewModel>
    {
        private readonly ICurrencyService currencyService;
        private readonly IPartnerService partnerService;

        public UpdateCurrencyGroupPageHandler(IMapper mapper, ICurrencyGroupApi api, ICurrencyService currencyService, IPartnerService partnerService) : base(mapper, api)
        {
            this.currencyService = currencyService;
            this.partnerService = partnerService;
        }

        public async Task<UpdateCurrencyGroupViewModel> Handle(UpdateCurrencyGroupPageRequest request, CancellationToken cancellationToken)
        {
            var group = await Api.GetById(request.Id);
            Func<CurrencyModel, bool> filterCurrency = (model) => (string.IsNullOrEmpty(model.GroupId) || string.Equals(model.GroupId, group.Id)) && model.Enabled;

            var parterItems = await partnerService.GetPartnersForSelection(partner => true);
            var currencies = await currencyService.GetCurrenciesForSelection(filterCurrency);

            return new UpdateCurrencyGroupViewModel
            {
                PartnerId = group.PartnerId,
                CurrencyItems = currencies,
                Currencies = group.Currencies.Select(item => item.Id).ToList(),
                PartnerItems = parterItems,
                Id = group.Id,
                Enabled = group.Enabled,
                Name = group.Name,
                FractionalOddsFormat = group.FractionalOddsFormat
            };
        }
    }
}