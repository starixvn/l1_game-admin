using AutoMapper;
using GamesAdmin.Api.Public.Models;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.Public.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GameConfigEntity, GameConfig>();
        }
    }
}