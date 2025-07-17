using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;

namespace GamesAdmin.Api.CurrencyGroupGames.Mappers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CurrencyGroupGameEntity, CurrencyGroupGame>()
                 .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId.ToString()));
            CreateMap<CurrencyGroupGame, CurrencyGroupGameEntity>()
                 .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => ObjectId.Parse(src.GroupId)));
            CreateMap<CurrencyGroupGameConfigEntity, CurrencyGroupGameConfig>();
            CreateMap<CurrencyGroupGameConfig, CurrencyGroupGameConfigEntity>();
        }
    }
}