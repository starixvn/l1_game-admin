using AutoMapper;
using GamesAdmin.Site.Features.ExchangeRates.Models;
using GamesAdmin.Site.Features.ExchangeRates.Requests;

namespace GamesAdmin.Site.Features.ExchangeRates.Mappers
{
    public class ExchangeRateMapper : Profile
    {
        public ExchangeRateMapper()
        {
            CreateMap<QueryExchangeRateRequest, QueryExchangeRateModel>()
                .ConstructUsing(request => new QueryExchangeRateModel(request.From, request.To));
        }
    }
}