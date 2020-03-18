using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<GameConfigEntity, GameConfig>()
                 .ForMember(dest => dest.DisabledRound, opt => opt.MapFrom(src => src.disabledround)); 

            CreateMap<GameConfig, GameConfigEntity>();
        }
    }
}
