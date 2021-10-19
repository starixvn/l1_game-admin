using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GamesAdmin.Api.Currencies;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;

namespace GamesAdmin.Api.CurrencyGroups.Handlers
{
    public class Handler
    {
        public Handler(IMapper mapper, IGenericRepository<CurrencyGroupEntity> repository, ICurrencyService currency)
        {
            Repository = repository;
            Mapper = mapper;
            Currency = currency;
        }

        public IGenericRepository<CurrencyGroupEntity> Repository { get; }

        public IMapper Mapper { get; }

        public ICurrencyService Currency { get; }

        protected static CurrencyGroupModel TransformToModel(CurrencyGroupEntity entity, IEnumerable<CurrencyModel> currencies)
        {
            var currenciesInGroup = currencies.Where(currency => string.Equals(currency.GroupId, entity.Id.ToString()));

            return new CurrencyGroupModel(entity.Id.ToString(), entity.Name, entity.Enabled, entity.PartnerId, currenciesInGroup);
        }
    }
}