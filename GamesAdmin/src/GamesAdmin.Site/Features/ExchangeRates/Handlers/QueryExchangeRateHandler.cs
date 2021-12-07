using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Site.Features.ExchangeRates.Models;
using GamesAdmin.Site.Features.ExchangeRates.Requests;
using GamesAdmin.Site.Features.ExchangeRates.ViewModels;
using MediatR;

namespace GamesAdmin.Site.Features.ExchangeRates.Handlers
{
    public class QueryExchangeRateHandler : ExchangeRateHandler, IRequestHandler<QueryExchangeRateRequest, ExchangeRateManagementViewModel>
    {
        public QueryExchangeRateHandler(IExchangeRateApi api, IMapper mapper) : base(api, mapper)
        {
        }

        public async Task<ExchangeRateManagementViewModel> Handle(QueryExchangeRateRequest request, CancellationToken cancellationToken)
        {
            var model = Mapper.Map<QueryExchangeRateModel>(request);
            var rates = await Api.Query(model);
            var viewModel = new ExchangeRateManagementViewModel
            {
                ExchangeRates = rates
            };

            return viewModel;
        }
    }
}