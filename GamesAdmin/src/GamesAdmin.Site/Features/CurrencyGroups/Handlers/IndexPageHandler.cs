using System.Collections.Generic;
using System.Linq;
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
    public class IndexPageHandler : Handler, IRequestHandler<IndexPageRequest, IndexViewModel>
    {
        private readonly ICurrencyService currencyService;
        private readonly IPartnerService partnerService;

        public IndexPageHandler(IMapper mapper, ICurrencyGroupApi api, ICurrencyService currencyService, IPartnerService partnerService) : base(mapper, api)
        {
            this.currencyService = currencyService;
            this.partnerService = partnerService;
        }

        public async Task<IndexViewModel> Handle(IndexPageRequest request, CancellationToken cancellationToken)
        {
            var currencies = await currencyService.GetAll();
            var partners = await partnerService.GetAll();
            var currencyItems = await currencyService.GetCurrenciesForSelection(model => true);

            return new IndexViewModel
            {
                Currencies = currencyItems.Select(item => item.Value).AsEnumerable(),
                CurrencyModels = currencies,
                Partners = partners,
                CurrencyItems = currencyItems
            };
        }
    }
}