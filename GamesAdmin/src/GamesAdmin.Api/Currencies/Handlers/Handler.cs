using AutoMapper;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities.Currencies;

namespace GamesAdmin.Api.Currencies.Handlers
{
    public class Handler
    {
        public Handler(IMapper mapper, IGenericRepository<CurrencyEntity> repository)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public IGenericRepository<CurrencyEntity> Repository { get; }

        public IMapper Mapper { get; }
    }
}