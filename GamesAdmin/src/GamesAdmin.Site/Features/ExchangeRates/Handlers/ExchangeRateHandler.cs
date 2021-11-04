using AutoMapper;

namespace GamesAdmin.Site.Features.ExchangeRates.Handlers
{
    public abstract class ExchangeRateHandler
    {
        protected ExchangeRateHandler(IExchangeRateApi api, IMapper mapper)
        {
            Api = api;
            Mapper = mapper;
        }

        public IExchangeRateApi Api { get; }

        public IMapper Mapper { get; }
    }
}