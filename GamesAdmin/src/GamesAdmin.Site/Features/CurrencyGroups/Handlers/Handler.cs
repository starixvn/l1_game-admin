using AutoMapper;

namespace GamesAdmin.Site.Features.CurrencyGroups.Handlers
{
    public class Handler
    {
        public Handler(IMapper mapper, ICurrencyGroupApi api)
        {
            Api = api;
            Mapper = mapper;
        }

        public ICurrencyGroupApi Api { get; }

        public IMapper Mapper { get; }
    }
}