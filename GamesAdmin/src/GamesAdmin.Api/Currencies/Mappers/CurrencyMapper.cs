using AutoMapper;
using GamesAdmin.Api.Currencies.Requests;
using GamesAdmin.Core.Models.Currencies;
using GamesAdmin.Database.Entities.Currencies;
using MongoDB.Bson;

namespace GamesAdmin.Api.Currencies.Mappers
{
    public class CurrencyMapper : Profile
    {
        public CurrencyMapper()
        {
            CreateMap<CreateCurrencyModel, CurrencyEntity>()
                .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => ObjectId.Parse(src.GroupId)));
            CreateMap<CurrencyModel, UpdateCurrencyModel>();
            CreateMap<CurrencyEntity, CurrencyModel>()
                .ConstructUsing(entity => new CurrencyModel(
                    entity.Id.ToString(),
                    entity.Name,
                    entity.Code,
                    entity.Multiplier,
                    entity.Enabled,
                    entity.IsTest,
                    entity.PartnerId,
                    entity.GroupId.Equals(ObjectId.Empty) ? string.Empty : entity.GroupId.ToString()));
        }
    }
}