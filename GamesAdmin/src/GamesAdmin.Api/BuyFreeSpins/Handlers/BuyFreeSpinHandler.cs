using AutoMapper;
using GamesAdmin.Api.ChatBot;
using GamesAdmin.Database;
using GamesAdmin.Database.Entities;
using Sentry;

namespace GamesAdmin.Api.BuyFreeSpins.Handlers
{
    public class BuyFreeSpinHandler
    {
        public BuyFreeSpinHandler(IMapper mapper, IGenericRepository<GameConfigEntity> repository, ISentryClient sentry, IChatBotService chatBotService)
        {
            Repository = repository;
            Mapper = mapper;
            Sentry = sentry;
            ChatBotService = chatBotService;
        }

        public IGenericRepository<GameConfigEntity> Repository { get; }

        public IMapper Mapper { get; }
        protected ISentryClient Sentry { get; }
        protected IChatBotService ChatBotService { get; }

    }
}