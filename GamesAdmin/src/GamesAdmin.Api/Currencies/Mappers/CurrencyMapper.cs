using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database.Entities.Currencies;

namespace GamesAdmin.Api.Currencies.Mappers
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
        {
            CreateMap<CreateCurrencyModel, CurrencyEntity>();
            CreateMap<CurrencyModel, UpdateCurrencyModel>();
            CreateMap<CurrencyEntity, CurrencyModel>()
                .ConstructUsing(entity => new CurrencyModel(
                    entity.Id.ToString(),
                    entity.Name,
                    entity.Code,
                    entity.Rate,
                    entity.Enabled,
                    entity.IsTest,
                    entity.PartnerId,
                    entity.GroupId));
        }
    }
}