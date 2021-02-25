using AutoMapper;
using GamesAdmin.Core.Models;
using GamesAdmin.Database.Entities;

namespace GamesAdmin.Api.GlobalConfigs.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GlobalConfigEntity, GlobalConfig>().ReverseMap();
        }
    }
}