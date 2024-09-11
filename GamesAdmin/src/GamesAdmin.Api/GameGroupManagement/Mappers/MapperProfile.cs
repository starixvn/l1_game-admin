using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities.GameGroup;

namespace GamesAdmin.Api.CurrencyGroupGames.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GameGroupEntity, GameGroup>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
}