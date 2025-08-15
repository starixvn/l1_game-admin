using AutoMapper;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class BuyFreeSpinHandler
    {
        public BuyFreeSpinHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry)
        {
            Repository = repository;
            Mapper = mapper;
            Sentry = sentry;
        }

        public IGenericRepository<GameConfigEntity> Repository { get; }

        public IMapper Mapper { get; }
        protected ISentryClient Sentry { get; }

    }
}