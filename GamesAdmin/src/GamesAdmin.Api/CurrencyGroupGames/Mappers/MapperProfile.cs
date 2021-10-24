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
                 .ForMember(dest => dest.CurrencyGroupId, opt => opt.MapFrom(src => src.CurrencyGroupId.ToString())); ;
            CreateMap<CurrencyGroupGame, CurrencyGroupGameEntity>()
                 .ForMember(dest => dest.CurrencyGroupId, opt => opt.MapFrom(src => ObjectId.Parse(src.CurrencyGroupId))); ;
        }
    }
}