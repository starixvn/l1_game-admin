using AutoMapper;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.ExchangeRates;

namespace GamesAdmin.Api.ExchangeRates.Handlers
{
    public abstract class ExchangeRateHandler
    {
        protected ExchangeRateHandler(IMapper mapper, IGenericRepository<ExchangeRateEntity> repository)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public IGenericRepository<ExchangeRateEntity> Repository { get; }

        public IMapper Mapper { get; }
    }
}