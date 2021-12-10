using AutoMapper;

namespace GamesAdmin.Site.Features.Currencies.Handlers
{
    public class Handler
    {
        public Handler(IMapper mapper, ICurrencyApi api)
        {
            Api = api;
            Mapper = mapper;
        }

        public ICurrencyApi Api { get; }

        public IMapper Mapper { get; }
    }
}