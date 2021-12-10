using AutoMapper;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;
using Sentry;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class Handler
    {
        public Handler(IMapper mapper, IGenericRepository<CurrencyEntity> repository, ISentryClient sentry)
        {
            Repository = repository;
            Mapper = mapper;
            Sentry = sentry;
        }

        public IGenericRepository<CurrencyEntity> Repository { get; }

        public IMapper Mapper { get; }

        public ISentryClient Sentry { get; }
    }
}