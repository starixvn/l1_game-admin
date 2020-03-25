using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;
using MongoDB.Bson;

namespace GamesAdmin.Api._Shared
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<GameConfigEntity, GameConfig>()
                 .ForMember(dest => dest.DisabledRound, opt => opt.MapFrom(src => src.disabledround));

            CreateMap<GameConfig, GameConfigEntity>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()));

            CreateMap<MiniSicboRoundEntity, Round>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.number));
            CreateMap<MiniSicboOnesEntity, BetInfo>();
        }
    }
}
