using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Api.CurrencyGroups.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using MediatR;
using Sentry;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class GetByCurrenciesHandler : Handler, IRequestHandler<GetByCurrenciesRequest, IEnumerable<CurrencyGroupModel>>
    {
        public GetByCurrenciesHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency, ISentryClient sentry)
            : base(mapper, repository, currency, sentry)
        {
        }

        public async Task<IEnumerable<CurrencyGroupModel>> Handle(GetByCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var currencies = await Currency.GetAll();
            var acceptedCurrencies = currencies.Where(currency => model.Currencies.Any(currencyId => string.Equals(currencyId, currency.Id))).ToList();
            var allEntities = await Repository.FilterByAsync(entity => true);
            var entities = allEntities.AsEnumerable().Where(entity
                => acceptedCurrencies.Any(currency => string.Equals(currency.GroupId, entity.Id.ToString())));

            return entities.Select(entity => TransformToModel(entity, currencies)).ToList();
        }
    }
}