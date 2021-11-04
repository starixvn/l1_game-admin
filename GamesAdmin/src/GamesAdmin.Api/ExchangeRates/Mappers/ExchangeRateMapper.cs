using System.Collections.Generic;
using AutoMapper;
using GamesAdmin.Api.ExchangeRates.Requests;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities.ExchangeRates;

namespace GamesAdmin.Api.ExchangeRates.Mappers
{
    public class ExchangeRateMapper : Profile
    {
        public ExchangeRateMapper()
        {
            CreateMap<ExchangeRateEntity, ExchangeRate>()
                .ConstructUsing(entity => new ExchangeRate(entity.Time, entity.ExId, entity.Source, new Dictionary<string, double>(entity.Code)));

            CreateMap<QueryExchangeRateModel, QueryExchangeRateRequest>()
                .ConstructUsing(model => new QueryExchangeRateRequest(model.From, model.To));
        }
    }
}