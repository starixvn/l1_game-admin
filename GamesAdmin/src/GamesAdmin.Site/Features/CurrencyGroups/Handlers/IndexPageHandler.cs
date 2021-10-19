using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.Currencies;
using GamesAdmin.Site.Features.Currencies.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.Requests;
using GamesAdmin.Site.Features.CurrencyGroups.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class IndexPageHandler : Handler, IRequestHandler<IndexPageRequest, IndexViewModel>
    {
        private readonly IMediator mediator;
        private readonly ICurrencyService currencyService;

        public IndexPageHandler(IMapper mapper, ICurrencyGroupApi api, IMediator mediator, ICurrencyService currencyService) : base(mapper, api)
        {
            this.mediator = mediator;
            this.currencyService = currencyService;
        }

        public async Task<IndexViewModel> Handle(IndexPageRequest request, CancellationToken cancellationToken)
        {
            var partners = await mediator.Send(new GetPartnersForSelectionRequest());
            var currencies = await currencyService.GetCurrenciesForSelection(model => true);

            return new IndexViewModel
            {
                PartnerItems = partners,
                CurencyItems = currencies,
                Partners = partners.Select(partner => partner.Value),
                Currencies = currencies.Select(currency => currency.Value)
            };
        }
    }
}