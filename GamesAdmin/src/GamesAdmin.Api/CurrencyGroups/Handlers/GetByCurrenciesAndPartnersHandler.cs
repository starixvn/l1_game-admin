using System;
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

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class GetByCurrenciesAndPartnersHandler : Handler, IRequestHandler<GetByCurrenciesAndPartnersRequest, IEnumerable<CurrencyGroupModel>>
    {
        public GetByCurrenciesAndPartnersHandler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency)
            : base(mapper, repository, currency)
        {
        }

        public async Task<IEnumerable<CurrencyGroupModel>> Handle(GetByCurrenciesAndPartnersRequest request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var currencies = await Currency.GetAll();
            var acceptedCurrencies = currencies.Where(currency => model.Currencies.Any(currencyId => string.Equals(currencyId, currency.Id))).ToList();
            var allEntities = await Repository.FilterByAsync(entity => true);
            var entities = allEntities.AsEnumerable().Where(entity
                => model.Partners.Any(partner => string.Equals(partner, entity.PartnerId))
                  && acceptedCurrencies.Any(currency => string.Equals(currency.GroupId, entity.Id.ToString())));

            return entities.Select(entity => TransformToModel(entity, currencies)).ToList();
        }
    }
}